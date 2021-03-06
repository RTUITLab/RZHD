﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RZHD.Data;
using RZHD.Models;
using RZHD.Models.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.Configure.Models.Configure.Interfaces;

namespace RZHD.Services.Configure
{
    public class FillDb : IConfigureWork
    {
        private readonly DatabaseContext context;
        private readonly UserManager<User> userManager;
        private readonly FillDbOptions options;

        public FillDb(DatabaseContext context, IOptions<FillDbOptions> options,
            UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager;
            this.options = options.Value;
        }
        public async Task Configure()
        {
            await AddDefaultUser();
            await AddStations();
            await AddRestaurants();
            await AddTickets();
            await AddTrains();


            await AddLinks();
        }
        private async Task AddLinks()
        {
            var sts = await context.Stations.ToListAsync();
            int i = 0;
            // tickets
            foreach (var ticket in context.Tickets)
            {
                ticket.Stations = new List<StationTicket>();
                ticket.Stations.Add(new StationTicket
                {
                    StationId = sts[i % 2 == 0 ? 0 : 1].Id,
                    Station = sts[i % 2 == 0 ? 0 : 1],
                    TicketId = ticket.Id,
                    Ticket = ticket
                });
                ticket.Stations.Add(new StationTicket
                {
                    StationId = sts[i % 2 == 0 ? 5 : 7].Id,
                    Station = sts[i % 2 == 0 ? 5 : 7],
                    TicketId = ticket.Id,
                    Ticket = ticket
                });
                ticket.Stations.Add(new StationTicket
                {
                    StationId = sts[i % 2 == 0 ? 7 : 2].Id,
                    Station = sts[i % 2 == 0 ? 7 : 2],
                    TicketId = ticket.Id,
                    Ticket = ticket
                });
                ticket.Stations.Add(new StationTicket
                {
                    StationId = sts[i % 2 == 0 ? 3 : 4].Id,
                    Station = sts[i % 2 == 0 ? 3 : 4],
                    TicketId = ticket.Id,
                    Ticket = ticket
                });
                i++;
            }

            await context.SaveChangesAsync();

            i = 0;
            // restaurant
            foreach (var res in context.Restaurants)
            {
                res.DeliverStations = new List<StationRestaurant>();
                res.DeliverStations.Add(new StationRestaurant
                {
                    DeliverTime = TimeSpan.FromMinutes(new Random().Next(9, 17)),
                    RestaurantId = res.Id,
                    Restaurant = res,
                    StationId = sts[i % 2 == 0 ? 0 : 1].Id,
                    Station = sts[i % 2 == 0 ? 0 : 1]
                });
                res.DeliverStations.Add(new StationRestaurant
                {
                    DeliverTime = TimeSpan.FromMinutes(new Random().Next(27, 49)),
                    RestaurantId = res.Id,
                    Restaurant = res,
                    StationId = sts[i % 2 == 0 ? 5 : 4].Id,
                    Station = sts[i % 2 == 0 ? 5 : 4]
                });
                res.DeliverStations.Add(new StationRestaurant
                {
                    DeliverTime = TimeSpan.FromMinutes(new Random().Next(27, 49)),
                    RestaurantId = res.Id,
                    Restaurant = res,
                    StationId = sts[i % 2 == 0 ? 6 : 5].Id,
                    Station = sts[i % 2 == 0 ? 6 : 5]
                });
                i++;
            }

            await context.SaveChangesAsync();

            // trains
            i = 0;
            foreach (var train in context.Trains)
            {
                train.Stations = new List<StationTrain>();
                train.Stations.Add(new StationTrain
                {
                    ArriveTime = train.DepartureTime + TimeSpan.FromMinutes(new Random().Next(60, 78)),
                    TrainId = train.Id,
                    Train = train,
                    StationId = sts[i % 2 == 0 ? 0 : 1].Id,
                    Station = sts[i % 2 == 0 ? 0 : 1]
                });
                train.Stations.Add(new StationTrain
                {
                    ArriveTime = train.DepartureTime + TimeSpan.FromMinutes(new Random().Next(42, 60)),
                    TrainId = train.Id,
                    Train = train,
                    StationId = sts[i % 2 == 0 ? 3 : 5].Id,
                    Station = sts[i % 2 == 0 ? 3 : 5]
                });
                train.Stations.Add(new StationTrain
                {
                    ArriveTime = train.DepartureTime + TimeSpan.FromMinutes(new Random().Next(60, 78)),
                    TrainId = train.Id,
                    Train = train,
                    StationId = sts[i % 2 == 0 ? 7 : 6].Id,
                    Station = sts[i % 2 == 0 ? 7 : 6]
                });
                train.Stations.Add(new StationTrain
                {
                    ArriveTime = train.DepartureTime + TimeSpan.FromMinutes(new Random().Next(42, 60)),
                    TrainId = train.Id,
                    Train = train,
                    StationId = sts[i % 2 == 0 ? 2 : 4].Id,
                    Station = sts[i % 2 == 0 ? 2 : 4]
                });
                train.Stations.Add(new StationTrain
                {
                    ArriveTime = train.DepartureTime + TimeSpan.FromMinutes(new Random().Next(42, 60)),
                    TrainId = train.Id,
                    Train = train,
                    StationId = sts[i % 2 == 0 ? 1 : 2].Id,
                    Station = sts[i % 2 == 0 ? 1 : 2]
                });
                train.Stations.Add(new StationTrain
                {
                    ArriveTime = train.DepartureTime + TimeSpan.FromMinutes(new Random().Next(42, 60)),
                    TrainId = train.Id,
                    Train = train,
                    StationId = sts[i % 2 == 0 ? 4 : 3].Id,
                    Station = sts[i % 2 == 0 ? 4 : 3]
                });
                train.Stations.Add(new StationTrain
                {
                    ArriveTime = train.DepartureTime + TimeSpan.FromMinutes(new Random().Next(42, 60)),
                    TrainId = train.Id,
                    Train = train,
                    StationId = sts[i % 2 == 0 ? 5 : 0].Id,
                    Station = sts[i % 2 == 0 ? 5 : 0]
                });
                train.Stations.Add(new StationTrain
                {
                    ArriveTime = train.DepartureTime + TimeSpan.FromMinutes(new Random().Next(42, 60)),
                    TrainId = train.Id,
                    Train = train,
                    StationId = sts[i % 2 == 0 ? 6 : 7].Id,
                    Station = sts[i % 2 == 0 ? 6 : 7]
                });
                i++;
            }

            await context.SaveChangesAsync();
        }

        private async Task AddTrains()
        {
            foreach (var train in context.Trains)
                context.Trains.Remove(train);

            context.Trains.Add(new Train
            {
                Number = num1,
                WagonsNumber = new Random().Next(1, 23),
                ArriveTIme = new DateTime(2019, 10, 2, 0, 0, 0),
                DepartureTime = new DateTime(2019, 9, 28, 20, 50, 0)
            });
            context.Trains.Add(new Train
            {
                Number = num2,
                WagonsNumber = new Random().Next(1, 23),
                ArriveTIme = new DateTime(2019, 10, 3, 10, 2, 0),
                DepartureTime = new DateTime(2019, 9, 28, 21, 51, 0)
            });
            context.Trains.Add(new Train
            {
                Number = num3,
                WagonsNumber = new Random().Next(1, 23),
                ArriveTIme = new DateTime(2019, 10, 1, 3, 7, 0),
                DepartureTime = new DateTime(2019, 9, 28, 20, 22, 0)
            });
            context.Trains.Add(new Train
            {
                Number = num4,
                WagonsNumber = new Random().Next(1, 23),
                ArriveTIme = new DateTime(2019, 10, 2, 11, 33, 0),
                DepartureTime = new DateTime(2019, 9, 28, 20, 49, 0)
            });
            context.Trains.Add(new Train
            {
                Number = num5,
                WagonsNumber = new Random().Next(1, 23),
                ArriveTIme = new DateTime(2019, 10, 3, 1, 1, 0),
                DepartureTime = new DateTime(2019, 9, 28, 21, 0, 0)
            });
            context.Trains.Add(new Train
            {
                Number = num6,
                WagonsNumber = new Random().Next(1, 23),
                ArriveTIme = new DateTime(2019, 10, 4, 5, 1, 0),
                DepartureTime = new DateTime(2019, 9, 28, 22, 0, 0)
            });
            context.Trains.Add(new Train
            {
                Number = num7,
                WagonsNumber = new Random().Next(1, 23),
                ArriveTIme = new DateTime(2019, 10, 8, 1, 1, 0),
                DepartureTime = new DateTime(2019, 9, 28, 20, 8, 0)
            });
            await context.SaveChangesAsync();
        }

        private string num1 = "ХХ1234567 123456";
        private string num2 = "АИ9872512 016253";
        private string num3 = "ОР9735535 186537";
        private string num4 = "НА0836574 928637";
        private string num5 = "УА2086537 896257";
        private string num6 = "z";
        private string num7 = "123";

        private async Task AddTickets()
        {
            foreach (var t in context.Tickets)
                context.Remove(t);

            await context.SaveChangesAsync();

            context.Tickets.Add(new Ticket
            {
                Number = num1,
                DepartureTime = new DateTime(2019, 9, 29, 9, 30, 0),
                ArriveTime = new DateTime(2019, 9, 28, 20, 50, 0),
                WagonNumber = new Random().Next(1, 23)
            });
            context.Tickets.Add(new Ticket
            {
                Number = num2,
                DepartureTime = new DateTime(2019, 9, 29, 5, 45, 0),
                ArriveTime = new DateTime(2019, 9, 28, 20, 57, 0),
                WagonNumber = new Random().Next(1, 23)
            });
            context.Tickets.Add(new Ticket
            {
                Number = num3,
                DepartureTime = new DateTime(2019, 9, 29, 3, 3, 0),
                ArriveTime = new DateTime(2019, 9, 28, 20, 47, 0),
                WagonNumber = new Random().Next(1, 23)
            });
            context.Tickets.Add(new Ticket
            {
                Number = num4,
                DepartureTime = new DateTime(2019, 9, 29, 10, 31, 0),
                ArriveTime = new DateTime(2019, 9, 28, 20, 49, 0),
                WagonNumber = new Random().Next(1, 23)
            });
            context.Tickets.Add(new Ticket
            {
                Number = num5,
                DepartureTime = new DateTime(2019, 9, 29, 9, 38, 0),
                ArriveTime = new DateTime(2019, 9, 28, 21, 42, 0),
                WagonNumber = new Random().Next(1, 23)
            });
            context.Tickets.Add(new Ticket
            {
                Number = num6,
                DepartureTime = new DateTime(2019, 9, 30, 1, 30, 0),
                ArriveTime = new DateTime(2019, 9, 28, 23, 39, 0),
                WagonNumber = new Random().Next(1, 23)
            });
            context.Tickets.Add(new Ticket
            {
                Number = num7,
                DepartureTime = new DateTime(2019, 10, 1, 11, 0, 0),
                ArriveTime = new DateTime(2019, 9, 29, 0, 0, 0),
                WagonNumber = new Random().Next(1, 23)
            });
            await context.SaveChangesAsync();
        }

        private async Task AddRestaurants()
        {
            foreach (var product in context.Products)
                context.Products.Remove(product);

            await context.SaveChangesAsync();

            foreach (var cat in context.Categories)
                context.Categories.Remove(cat);

            await context.SaveChangesAsync();

            foreach (var res in context.Restaurants)
                context.Restaurants.Remove(res);

            await context.SaveChangesAsync();

            context.Restaurants.Add(new Restaurant
            {
                Name = "Русский повар",
                ImageUrl = "https://www.delivery-club.ru/naturmort/44000090_480x300.jpg&quot",
                Menu = new List<Category>
                {
                    new Category
                    {
                        Name = "Тесто и мясо",
                        Products = new List<Product>
                        {
                            new Product
                            {
                                Name = "Чебурек",
                                Price = 99,
                                ImageUrl = "https://www.delivery-club.ru/media/cms/relation_product/11267/304598730_m200.jpg"
                            },
                            new Product
                            {
                                Name = "Пельмени",
                                Price = 99,
                                ImageUrl = "https://www.delivery-club.ru/media/cms/relation_product/11267/304598730_m200.jpg"
                            },
                            new Product
                            {
                                Name = "Булочка с мясом",
                                Price = 39,
                                ImageUrl = "https://www.delivery-club.ru/media/cms/relation_product/11267/304598730_m200.jpg"
                            }
                        }
                    }
                }
            });

            context.Restaurants.Add(new Restaurant
            {
                Name = "Якитория",
                ImageUrl = "https://www.delivery-club.ru/naturmort/2000013_480x300.jpg&quot",
                Menu = new List<Category>
                {
                    new Category
                    {
                        Name = "Роллы",
                        Products = new List<Product>
                        {
                            new Product
                            {
                                Name = "Горячие",
                                Price = 109,
                                ImageUrl = "https://www.delivery-club.ru/media/cms/relation_product/11267/304598730_m200.jpg"
                            },
                            new Product
                            {
                                Name = "Филадельфия",
                                Price = 149,
                                ImageUrl = "https://www.delivery-club.ru/media/cms/relation_product/11267/304598730_m200.jpg"
                            },
                            new Product
                            {
                                Name = "Самурай",
                                Price = 99,
                                ImageUrl = "https://www.delivery-club.ru/media/cms/relation_product/11267/304598730_m200.jpg"
                            },
                            new Product
                            {
                                Name = "С курицей",
                                Price = 19,
                                ImageUrl = "https://www.delivery-club.ru/media/cms/relation_product/11267/304598730_m200.jpg"
                            }
                        }
                    },
                    new Category
                    {
                        Name = "Суши",
                        Products = new List<Product>
                        {
                            new Product
                            {
                                Name = "Большой",
                                Price = 79,
                                ImageUrl = "https://www.delivery-club.ru/media/cms/relation_product/11267/304598730_m200.jpg"
                            },
                            new Product
                            {
                                Name = "Вкусный",
                                Price = 49,
                                ImageUrl = "https://www.delivery-club.ru/media/cms/relation_product/11267/304598730_m200.jpg"
                            }
                        }
                    }
                }
            });

            context.Restaurants.Add(new Restaurant
            {
                Name = "Бургеркинг",
                ImageUrl = "https://www.delivery-club.ru/naturmort/59a01bcda3f70_480x300.jpg&quot",
                Menu = new List<Category>
                {
                    new Category
                    {
                        Name = "Бургеры",
                        Products = new List<Product>
                        {
                            new Product
                            {
                                Name = "Кинг сайз",
                                Price = 99,
                                ImageUrl = "https://www.delivery-club.ru/media/cms/relation_product/11267/304598730_m200.jpg"
                            },
                            new Product
                            {
                                Name = "Кинг не сайз",
                                Price = 59,
                                ImageUrl = "https://www.delivery-club.ru/media/cms/relation_product/11267/304598730_m200.jpg"
                            },
                            new Product
                            {
                                Name = "Невкусный",
                                Price = 199,
                                ImageUrl = "https://www.delivery-club.ru/media/cms/relation_product/11267/304598730_m200.jpg"
                            },
                            new Product
                            {
                                Name = "Красивый",
                                Price = 49,
                                ImageUrl = "https://www.delivery-club.ru/media/cms/relation_product/11267/304598730_m200.jpg"
                            }
                        }
                    },
                    new Category
                    {
                        Name = "Картофель",
                        Products = new List<Product>
                        {
                            new Product
                            {
                                Name = "Картофель фри",
                                Price = 49,
                                ImageUrl = "https://www.delivery-club.ru/media/cms/relation_product/11267/304598730_m200.jpg"
                            },
                            new Product
                            {
                                Name = "Картофель по дер-ки",
                                Price = 59,
                                ImageUrl = "https://www.delivery-club.ru/media/cms/relation_product/11267/304598730_m200.jpg"
                            }
                        }
                    }
                }
            });

            context.Restaurants.Add(new Restaurant
            {
                Name = "Казахстан",
                ImageUrl = "https://www.delivery-club.ru/naturmort/43000082_480x300.jpg&quot",
                Menu = new List<Category>
                {
                    new Category
                    {
                        Name = "С зеленью",
                        Products = new List<Product>
                        {
                            new Product
                            {
                                Name = "Очень вкусно",
                                Price = 299,
                                ImageUrl = "https://www.delivery-club.ru/media/cms/relation_product/11267/304598730_m200.jpg"
                            },
                            new Product
                            {
                                Name = "Средней вкусности",
                                Price = 159,
                                ImageUrl = "https://www.delivery-club.ru/media/cms/relation_product/11267/304598730_m200.jpg"
                            },
                            new Product
                            {
                                Name = "Собери сам",
                                Price = 99,
                                ImageUrl = "https://www.delivery-club.ru/media/cms/relation_product/11267/304598730_m200.jpg"
                            }
                        }
                    }
                }
            });

            context.Restaurants.Add(new Restaurant
            {
                Name = "МакДоналдс",
                ImageUrl = "https://www.delivery-club.ru/naturmort/5d43e992e1d52_480x300.jpg&quot",
                Menu = new List<Category>
                {
                    new Category
                    {
                        Name = "Бургеры",
                        Products = new List<Product>
                        {
                            new Product
                            {
                                Name = "Биг мак",
                                Price = 99,
                                ImageUrl = "https://www.delivery-club.ru/media/cms/relation_product/11267/304598730_m200.jpg"
                            },
                            new Product
                            {
                                Name = "Чизбургер",
                                Price = 59,
                                ImageUrl = "https://www.delivery-club.ru/media/cms/relation_product/11267/304598730_m200.jpg"
                            },
                            new Product
                            {
                                Name = "Макчикен",
                                Price = 199,
                                ImageUrl = "https://www.delivery-club.ru/media/cms/relation_product/11267/304598730_m200.jpg"
                            },
                            new Product
                            {
                                Name = "Фишбургер",
                                Price = 49,
                                ImageUrl = "https://www.delivery-club.ru/media/cms/relation_product/11267/304598730_m200.jpg"
                            }
                        }
                    },
                    new Category
                    {
                        Name = "Картофель",
                        Products = new List<Product>
                        {
                            new Product
                            {
                                Name = "Картофель фри",
                                Price = 49,
                                ImageUrl = "https://www.delivery-club.ru/media/cms/relation_product/11267/304598730_m200.jpg"
                            },
                            new Product
                            {
                                Name = "Картофель по дер-ки",
                                Price = 59,
                                ImageUrl = "https://www.delivery-club.ru/media/cms/relation_product/11267/304598730_m200.jpg"
                            }
                        }
                    }
                }
            });

            context.Restaurants.Add(new Restaurant
            {
                Name = "КФС",
                ImageUrl = "https://www.delivery-club.ru/naturmort/5ca31e7b22399_480x300.jpg&quot",
                Menu = new List<Category>
                {
                    new Category
                    {
                        Name = "Наборы",
                        Products = new List<Product>
                        {
                            new Product
                            {
                                Name = "Собери сам",
                                Price = 199,
                                ImageUrl = "https://www.delivery-club.ru/media/cms/relation_product/11267/304598730_m200.jpg"
                            },
                            new Product
                            {
                                Name = "6 штук",
                                Price = 166,
                                ImageUrl = "https://www.delivery-club.ru/media/cms/relation_product/11267/304598730_m200.jpg"
                            },
                            new Product
                            {
                                Name = "Выгодный",
                                Price = 99,
                                ImageUrl = "https://www.delivery-club.ru/media/cms/relation_product/11267/304598730_m200.jpg"
                            }
                        }
                    },
                    new Category
                    {
                        Name = "Баскет",
                        Products = new List<Product>
                        {
                            new Product
                            {
                                Name = "Большой",
                                Price = 299,
                                ImageUrl = "https://www.delivery-club.ru/media/cms/relation_product/11267/304598730_m200.jpg"
                            },
                            new Product
                            {
                                Name = "Средний",
                                Price = 159,
                                ImageUrl = "https://www.delivery-club.ru/media/cms/relation_product/11267/304598730_m200.jpg"
                            },
                            new Product
                            {
                                Name = "Маленький",
                                Price = 109,
                                ImageUrl = "https://www.delivery-club.ru/media/cms/relation_product/11267/304598730_m200.jpg"
                            }
                        }
                    }
                }
            });

            await context.SaveChangesAsync();
        }

        private async Task AddStations()
        {
            foreach (var st in context.Stations)
                context.Stations.Remove(st);

            await context.SaveChangesAsync();

            context.Stations.Add(new Station
            {
                Name = "Нижний Новгород"
            });

            context.Stations.Add(new Station
            {
                Name = "Казанксий вокзал"
            });

            context.Stations.Add(new Station
            {
                Name = "Киров"
            });

            context.Stations.Add(new Station
            {
                Name = "Пермь"
            });

            context.Stations.Add(new Station
            {
                Name = "Санкт-Петербург"
            });

            context.Stations.Add(new Station
            {
                Name = "Ижевск"
            });

            context.Stations.Add(new Station
            {
                Name = "Сыктывкар"
            });

            context.Stations.Add(new Station
            {
                Name = "Подольск"
            });

            await context.SaveChangesAsync();
        }

        private async Task AddDefaultUser()
        {
            if (!await userManager.Users.AnyAsync(u => u.Email == options.Email))
            {
                var r = await userManager.CreateAsync(new User
                {
                    Firstname = options.Firstname,
                    Secondname = options.Secondname,
                    Middlename = options.Middlename,
                    Email = options.Email,
                    UserName = options.Email,
                    Age = options.Age,
                    BonusQuantity = options.BonusQuantity
                }, options.Password);
                Console.WriteLine("Create default user ------- " + r.Succeeded);
            }
        }
    }
}
