# A Discussion on Circuit Breaks in the Microservices Architecture: HttpClient Implementation

This repo is related to the series of articles: [A Discussion on Circuit Breaks in the Microservices Architecture](https://barrocaeric.medium.com/a-discussion-on-circuit-breaks-in-the-microservices-architecture-c0f45e6b37ca).

## Description

This repo shows how to implement a **Circuit Break Policy on a HttpClient for aspnet core applications**
using the [Polly](https://github.com/App-vNext/Polly) library. The benefits of this approach can be summarized as:
* Easy of Implementation
* Flexibility
* Easy Policy and Client Management

More details about the benefits and challenges of this implementation can be found on 
[A Discussion on Circuit Breaks in the Microservices Architecture: HttpClient Implementation](https://medium.com/@barrocaeric/a-discussion-on-circuit-breaks-in-the-microservices-architecture-httpclient-implementation-9c7211c4758e).

## This Repo

This repository contains the **main** and **the http-client-implementation** branches. 
The first one contains the main directions and explanations about this project. 
The **the http-client-implementation** contains the code implementation for the **Circuit Break Policy on one HttpClient**.

## The http-client-implementation branch

This branch contains 2 aspnet core applications **(User Service and Order Service)**. Both present the hierarchy below:
* **Controllers**: The controllers of the application.
* **Services** (optional): Business and Service Logic.
* **Models**: Data Models.
* **Settings**: Mock for Input Data through appsettings.Development.json.

The **Circuit Break** implementation is presented on the `PolyExtensions.cs` file on the Order Service application. It is a policy that implements a Circuit Break that reaches open state after two connection errors, and stays in that state for 1 minute.

To test the Circuit Break Implementation debug the Order Service application alone and try to access the endpoint `http://domain:port/order/1`. After two tries you should see on your Debug Console a log similar to the one bellow:

`warn: OrderService.Services.OrdersService[0]`

After 1 minute the Circuit Break will be on Half-Open State and able to try connection again. Run the User Service application and try again the `http://domain:port/order/1 endpoint`. Now you should see the result below:
 ```JSON
  {
    "id":1,
    "user":
    {
      "id":1,
      "name":"Anna",
      "address":"ABCD"
    }
  }
 ```
 ### Configuration
 Remember to set the applications url for debugging. Also, on the `Startup.cs` file of the Order Service application it is specified the `http://localhost:5000` url for the User Service HttpClient. Remember to replace it with your User Service application url.
