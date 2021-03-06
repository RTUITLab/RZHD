/* tslint:disable */
import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { BaseService } from '../base-service';
import { ApiConfiguration } from '../api-configuration';
import { StrictHttpResponse } from '../strict-http-response';
import { RequestBuilder } from '../request-builder';
import { Observable } from 'rxjs';
import { map, filter } from 'rxjs/operators';

import { LoginRequest } from '../models/login-request';
import { LoginView } from '../models/login-view';
import { RegisterRequest } from '../models/register-request';

@Injectable({
  providedIn: 'root',
})
export class AuthenticationService extends BaseService {
  constructor(
    config: ApiConfiguration,
    http: HttpClient
  ) {
    super(config, http);
  }

  /**
   * Path part for operation apiAuthRegisterPost
   */
  static readonly ApiAuthRegisterPostPath = '/api/auth/register';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiAuthRegisterPost$Json()` instead.
   *
   * This method sends `application/json-patch+json` and handles response body of type `application/json-patch+json`
   */
  apiAuthRegisterPost$Json$Response(params?: {

    body?: RegisterRequest
  }): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, AuthenticationService.ApiAuthRegisterPostPath, 'post');
    if (params) {


      rb.body(params.body, 'application/json-patch+json');
    }
    return this.http.request(rb.build({
      responseType: 'text',
      accept: '*/*'
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return (r as HttpResponse<any>).clone({ body: undefined }) as StrictHttpResponse<void>;
      })
    );
  }

  /**
   * This method provides access to only to the response body.
   * To access the full response (for headers, for example), `apiAuthRegisterPost$Json$Response()` instead.
   *
   * This method sends `application/json-patch+json` and handles response body of type `application/json-patch+json`
   */
  apiAuthRegisterPost$Json(params?: {

    body?: RegisterRequest
  }): Observable<void> {

    return this.apiAuthRegisterPost$Json$Response(params).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

  /**
   * Path part for operation apiAuthLoginPost
   */
  static readonly ApiAuthLoginPostPath = '/api/auth/login';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiAuthLoginPost$Json()` instead.
   *
   * This method sends `application/json-patch+json` and handles response body of type `application/json-patch+json`
   */
  apiAuthLoginPost$Json$Response(params?: {

    body?: LoginRequest
  }): Observable<StrictHttpResponse<LoginView>> {

    const rb = new RequestBuilder(this.rootUrl, AuthenticationService.ApiAuthLoginPostPath, 'post');
    if (params) {


      rb.body(params.body, 'application/json-patch+json');
    }
    return this.http.request(rb.build({
      responseType: 'json',
      accept: 'application/json'
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<LoginView>;
      })
    );
  }

  /**
   * This method provides access to only to the response body.
   * To access the full response (for headers, for example), `apiAuthLoginPost$Json$Response()` instead.
   *
   * This method sends `application/json-patch+json` and handles response body of type `application/json-patch+json`
   */
  apiAuthLoginPost$Json(params?: {

    body?: LoginRequest
  }): Observable<LoginView> {

    return this.apiAuthLoginPost$Json$Response(params).pipe(
      map((r: StrictHttpResponse<LoginView>) => r.body as LoginView)
    );
  }

}
