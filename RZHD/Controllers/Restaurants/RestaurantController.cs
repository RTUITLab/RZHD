﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RZHD.Data;
using RZHD.Models;
using RZHD.Models.Requests.Restaurant;
using RZHD.Models.Responses;
using RZHD.Models.Responses.Restaurants;
using RZHD.Models.Responses.Stations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RZHD.Controllers.Restaurants
{
    [Route("api/restaurant")]
    public class RestaurantController : MainController
    {
        private readonly DatabaseContext context;
        private readonly IMapper mapper;
        private readonly ILogger<RestaurantController> logger;

        public RestaurantController(DatabaseContext context, IMapper mapper, ILogger<RestaurantController> logger)
        {
            this.context = context;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet("{ticket}")]
        public async Task<ActionResult<Response<IEnumerable<RestaurantView>>>> Get(string ticket)
        {
            var response = new Response<List<RestaurantView>>
            {
                Status = false,
                Error = "Something went strange",
                Content = new List<RestaurantView>()
            };

            try
            {
                // ticket has all stations
                var tick = await context.Tickets.Where(t => t.Number == ticket)
                    .Include(t => t.Stations)
                    .SingleAsync();

                var time = new DateTime(2019, 9, 28, 21, 37, 0);

                List<RestaurantView> result = new List<RestaurantView>();

                foreach (var station in tick.Stations)
                {
                    // get all restaurants
                    var st = await context.Stations.Where(s => s.Id == station.StationId)
                        .Include(s => s.DeliverRestaurants)
                        .Include(s => s.Trains)
                        .SingleAsync();

                    if (st.Trains.Count == 0)
                        continue;


                    // get one train
                    StationTrain train = null;
                    foreach (var trai in st.Trains)
                    {
                        var temp = context.Trains.Where(tra => tra.Id == trai.TrainId).SingleAsync().Result;
                        if (temp.Number == ticket)
                        {
                            trai.Train = temp;
                            train = trai;
                            break;
                        }
                    }

                    if (train == null)
                        continue;

                    foreach (var restaurant in st.DeliverRestaurants)
                    {
                        // too late
                        if (train.ArriveTime < time)
                            continue;

                        // if less - no time
                        if (train.ArriveTime - time < restaurant.DeliverTime)
                            continue;

                        result.Add(new RestaurantView
                        {
                            Id = restaurant.RestaurantId,
                            Name = (await context.Restaurants.Where(res => res.Id == restaurant.RestaurantId).SingleAsync()).Name,
                            ImageUrl = (await context.Restaurants.Where(res => res.Id == restaurant.RestaurantId).SingleAsync()).ImageUrl,
                            StationTime = new List<StationTimeView>()
                        });
                        TimeSpan temp = train.ArriveTime - time;
                        string timeStr = temp.Days + " д. " + temp.Hours + " ч. " + temp.Minutes + " м.";
                        result.Last().StationTime.Add(new StationTimeView
                        {
                            Station = mapper.Map<StationView>(st),
                            Time = timeStr
                        });
                    }
                }

                result.ForEach(r => r.StationTime.OrderBy(stt => stt.Time));
                response.Content = result;
                response.Error = "";
                response.Status = true;
                return Ok(response);
            }
            catch (InvalidOperationException ioe)
            {
                logger.LogError(ioe.Message + "\n" + ioe.StackTrace);
                response.Error = "Билет не найден";
                return Ok(response);
            }
            catch (ArgumentNullException ane)
            {
                logger.LogError(ane.Message + "\n" + ane.StackTrace);
                response.Error = "Билет не найден";
                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message + "\n" + ex.StackTrace);
                response.Error = "Что-то пошло не так";
                return Ok(response);
            }
        }

        [HttpGet("{restId:int}")]
        public async Task<ActionResult<Response<IEnumerable<MenuView>>>> GetMenu(int restId)
        {
            var response = new Response<List<MenuView>>
            {
                Status = false,
                Error = "Something went strange",
                Content = new List<MenuView>()
            };

            try
            {
                var rest = await context.Restaurants.Where(r => r.Id == restId)
                    .Include(r => r.Menu)
                        .ThenInclude(m => m.Products)
                    .SingleAsync();

                List<MenuView> menu = new List<MenuView>();
                menu = mapper.Map<List<MenuView>>(rest.Menu);
                response.Status = true;
                response.Error = "";
                response.Content = menu;
                return Ok(response);
            }
            catch (InvalidOperationException ioe)
            {
                logger.LogError(ioe.Message + "\n" + ioe.StackTrace);
                response.Error = "Ресторан не найден";
                return Ok(response);
            }
            catch (ArgumentNullException ane)
            {
                logger.LogError(ane.Message + "\n" + ane.StackTrace);
                response.Error = "Ресторан не найден";
                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message + "\n" + ex.StackTrace);
                response.Error = "Что-то пошло не так";
                return Ok(response);
            }
        }

        [HttpPost("admin")]
        public async Task<ActionResult<Response<int>>> CreateOrder([FromBody] CreateOrderRequest createOrder)
        {
            var response = new Response<int>
            {
                Status = false,
                Error = "Something went strange",
                Content = -1
            };
            try
            {
                // get products from restaurants on these stations
                List<Product> products = new List<Product>();
                for (int i = 0; i < createOrder.ProductsId.Count; i++)
                    products.Add(await context.Products.Where(pr => pr.Id == createOrder.ProductsId[i]).SingleAsync());

                List<Station> stations = new List<Station>();
                for (int i = 0; i < createOrder.StationsId.Count; i++)
                    stations.Add(await context.Stations.Where(st => st.Id == createOrder.StationsId[i]).SingleAsync());

                List<Restaurant> restaurants = new List<Restaurant>();
                for (int i = 0; i < createOrder.RestaurantsId.Count; i++)
                    restaurants.Add(await context.Restaurants.Where(res => res.Id == createOrder.RestaurantsId[i]).SingleAsync());

                Order order = new Order
                {
                    TotalPrice = createOrder.TotalPrice,
                    Status = Status.ConfirmRequest,
                    Products = products,
                    Stations = stations,
                    Restaurants = restaurants
                };

                context.Orders.Add(order);
                await context.SaveChangesAsync();

                response.Status = true;
                response.Error = "";
                response.Content = order.Id;
                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message + "\n" + ex.StackTrace);
                response.Error = "Что-то пошло не так";
                return Ok(response);
            }
        }

        [HttpGet("adimn/{orderId:int}")]
        public async Task<ActionResult<Response<OrderView>>> GetInfoAboutOrder(int orderId)
        {
            var response = new Response<OrderView>
            {
                Status = false,
                Error = "Something went strange",
                Content = new OrderView()
            };
            try
            {
                Order order = await context.Orders.Where(or => or.Id == orderId).SingleAsync();
                OrderView orderView = mapper.Map<OrderView>(order);

                response.Status = true;
                response.Error = "";
                response.Content = orderView;
                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message + "\n" + ex.StackTrace);
                response.Error = "Что-то пошло не так";
                return Ok(response);
            }
        }
    }
}
