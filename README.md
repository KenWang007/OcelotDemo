## 创建Demo步骤

1.创建BookingApi： dotnet new -n BookingApi  
2.创建PassengerApi： dotnet new -n PassengerApi  
3.创建ApiGateway： dotnet new -n ApiGateway  
4.添加BookingApi和PassengerApi的实现代码  
5.在ApiGateway项目中用Nuget安装Ocelot依赖包  
6.添加configuration.json的配置文件  
7.配置路由响应规则  
8.启动服务并通过Api网关访问服务  

## 在学习之前你需要了解
服务发现：
反向代理：
API网关模式：https://www.cnblogs.com/xiandnc/p/9270365.html


## 为什么需要API网关？
API网关：系统的暴露在外部的一个访问入口。这个有点像代理访问的家伙，就像一个公司的门卫承担着寻址、限制进入、安全检查、位置引导、等功能
https://www.cnblogs.com/xiandnc/p/9270365.html

## Ocelot是什么？
Ocelot是基于.NET Core实现的轻量级API网关，它包括的主要功能有：路由、请求聚合、服务发现、认证、授权、限流熔断、并内置了LoadBanalce以及集成了Service Fabric、Consul、Eureka等功能，这些功能只都只需要简单的配置即可使用。

官网： http://threemammals.com/ocelot 
学习文档：http://ocelot.readthedocs.io/en/latest/introduction/gettingstarted.html

## Ocelot的实现机制
简单的来说它是一堆的asp.net core middleware组成的pipeline，当它拿到请求之后会用一个request builder来构造一个HttpRequestMessage发到下游的真实服务器，等下游的服务返回response之后再由一个middleware将它返回的HttpResponseMessage映射到HttpResponse上。

## Ocelot配置文件解析
{
  "ReRoutes": [
    {
      "DownstreamPathTemplate": "/api/booking",
      "UpstreamPathTemplate": "/api/getbookinginformation", 
      "UpstreamHttpMethod": [ "Get" ],
      "AddHeadersToRequest": {},
      "AddClaimsToRequest": {},
      "RouteClaimsRequirement": {},
      "AddQueriesToRequest": {},
      "RequestIdKey": "",
      "FileCacheOptions": {
        "TtlSeconds": 0,
        "Region": ""
      },
      "ReRouteIsCaseSensitive": false,
      "ServiceName": "",
      "DownstreamScheme": "http",
      "DownstreamHostAndPorts": [
        {
          "Host": "localhost",
          "Port": 8001
        }
      ],
      "QoSOptions": {
        "ExceptionsAllowedBeforeBreaking": 0,
        "DurationOfBreak": 0,
        "TimeoutValue": 0
      }
    }
  ],
  "GlobalConfiguration": {
    "BaseUrl": "https://localhost:5000"
  }
}

