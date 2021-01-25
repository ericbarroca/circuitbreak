# A Discussion on Circuit Breaks in the Microservices Architecture: Implementations

This repo is related to the series of articles: [A Discussion on Circuit Breaks in the Microservices Architecture](https://barrocaeric.medium.com/a-discussion-on-circuit-breaks-in-the-microservices-architecture-c0f45e6b37ca).

## Description

This repo shows how to implement a **Circuit Break Policy on a HttpClient for aspnet core applications**
using the [Polly](https://github.com/App-vNext/Polly) library. The benefits of this approach can be summarized as:
* Easy of Implementation
* Flexibility
* Easy Policy and Client Managment

More details about the benefits and challenges of this implementation can be found on 
[A Discussion on Circuit Breaks in the Microservices Architecture: HttpClient Implementation](https://medium.com/@barrocaeric/a-discussion-on-circuit-breaks-in-the-microservices-architecture-httpclient-implementation-9c7211c4758e).

## This Repo

This repository contains the **main** and **the http-client-implementation** branches. 
The first one contains the main directions and explanations about this project. 
The **the http-client-implementation** contains the code implementation for the **Circuit Break Policy on one HttpClient**.
