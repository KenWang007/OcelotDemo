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
服务发现：  https://www.cnblogs.com/xiaohanlin/p/8016803.html  
反向代理：  https://www.cnblogs.com/Anker/p/6056540.html  
API网关模式：https://www.cnblogs.com/xiandnc/p/9270365.html


## 为什么需要API网关？  
API网关：系统的暴露在外部的一个访问入口，类似于代理服务器，就像一个公司的门卫承担着寻址、限制进入、安全检查、位置引导、等功能  
https://www.cnblogs.com/xiandnc/p/9270365.html    

## Ocelot是什么？  
Ocelot是基于.NET Core实现的轻量级API网关，它包括的主要功能有：路由、请求聚合、服务发现、认证、授权、限流熔断、并内置了LoadBanalce以及集成了Service Fabric、Consul、Eureka等功能，这些功能只都只需要简单的配置即可使用。     
Ocelot在腾讯的使用：https://customers.microsoft.com/en-us/story/tencent-telecommunications-dotnetcore    
![图片名称](https://github.com/KenWang007/OcelotDemo/blob/master/Api%20Gateway%20.png)   

Github项目地址：https://github.com/ThreeMammals/Ocelot    
官网： http://threemammals.com/ocelot    
学习文档：http://ocelot.readthedocs.io/en/latest/introduction/gettingstarted.html   

## Ocelot的实现机制
简单的来说它是一堆的asp.net core middleware组成的pipeline，当它拿到请求之后会用一个request builder来构造一个HttpRequestMessage发到下游的真实服务器，等下游的服务返回response之后再由一个middleware将它返回的HttpResponseMessage映射到HttpResponse上。   
![图片名称](https://blog.johnwu.cc/images/pasted-114.gif)

## Ocelot配置文件解析

```javascript
{
  "ReRoutes": [ //路由是API网关最基本也是最核心的功能、ReRoutes下就是由多个路由节点组成。
    {
      "DownstreamPathTemplate": "", //下游服务模板
      "UpstreamPathTemplate": "", //上游服务模板
      "UpstreamHttpMethod": [ "Get" ],//上游方法类型Get,Post,Put
      "AddHeadersToRequest": {},//需要在转发过程中添加到Header的内容
      "FileCacheOptions": { //可以对下游请求结果进行缓存，主要依赖于CacheManager实现
        "TtlSeconds": 10,
        "Region": ""
      },
      "ReRouteIsCaseSensitive": false,//重写路由是否区分大小写
      "ServiceName": "",//服务名称
      "DownstreamScheme": "http",//下游服务schema：http, https
      "DownstreamHostAndPorts": [ //下游服务端口号和地址
        {
          "Host": "localhost",
          "Port": 8001
        }
      ],
      "RateLimitOptions": { //限流设置
        "ClientWhitelist": [], //客户端白名单
        "EnableRateLimiting": true,//是否启用限流设置
        "Period": "1s", //每次请求时间间隔
        "PeriodTimespan": 15,//恢复的时间间隔
        "Limit": 1 //请求数量
      }，
      "QoSOptions": { //服务质量与熔断,熔断的意思是停止将请求转发到下游服务。当下游服务已经出现故障的时候再请求也是无功而返，
      并且增加下游服务器和API网关的负担，这个功能是用的Polly来实现的，我们只需要为路由做一些简单配置即可
        "ExceptionsAllowedBeforeBreaking": 0, //允许多少个异常请求
        "DurationOfBreak": 0, //熔断的时间，单位为秒
        "TimeoutValue": 0 //如果下游请求的处理时间超过多少则自如将请求设置为超时
      }
    }
  ],
  "UseServiceDiscovery": false,//是否启用服务发现
  "Aggregates": [ //请求聚合
    {
      "ReRouteKeys": [ //设置需要聚合的路由key
        "booking",
        "passenger"
      ],
      "UpstreamPathTemplate": "/api/getbookingpassengerinfo" //暴露给外部的聚合请求路径
    },
  "GlobalConfiguration": { //全局配置节点
    "BaseUrl": "https://localhost:5000" //网关基地址
  }
}
```
当我们路由到的下游服务有多个结点的时候，我们可以在DownstreamHostAndPorts中进行配置负载
```javascript
{
    "DownstreamPathTemplate": "/api/posts/{postId}",
    "DownstreamScheme": "https",
    "DownstreamHostAndPorts": [
            {
                "Host": "10.0.1.10",
                "Port": 5000,
            },
            {
                "Host": "10.0.1.11",
                "Port": 5000,
            }
        ],
    "UpstreamPathTemplate": "/posts/{postId}",
    "LoadBalancer": "LeastConnection",
    "UpstreamHttpMethod": [ "Put", "Delete" ]
}
```
LoadBalancer将决定负载均衡的算法，目前支持下面三种方式  
1. LeastConnection – 将请求发往最空闲的那个服务器
2. RoundRobin – 轮流发送
3. NoLoadBalance – 总是发往第一个请求或者是服务发现

如果不希望对请求做任何的处理，则可以使用下面的万能模板：(万能模板的优先级最低，只要有其它的路由模板，其它的路由模板则会优先生效)
```javascript
{
    "DownstreamPathTemplate": "/{url}",
    "DownstreamScheme": "https",
    "DownstreamHostAndPorts": [
            {
                "Host": "localhost",
                "Port": 80,
            }
        ],
    "UpstreamPathTemplate": "/{url}",
    "UpstreamHttpMethod": [ "Get" ]
}
```
Prioirty优先级 :对多个产生冲突的路由设置优化级
```javascript
{
    "UpstreamPathTemplate": "/goods/{catchAll}"
    "Priority": 0
}
{
    "UpstreamPathTemplate": "/goods/delete"
    "Priority": 1
}
```
请求聚合：可以通过gateway将客户端的多个请求聚合然后将结果一次返回到客户端去，此时我们需要给每个模板指定一个key
```javascript
{
  "ReRoutes": [
    {
      "DownstreamPathTemplate": "/api/booking",
      "UpstreamPathTemplate": "/api/getbooking",
      "UpstreamHttpMethod": [ "Get" ],
      "DownstreamScheme": "http",
      "DownstreamHostAndPorts": [
        {
          "Host": "localhost",
          "Port": 8001
        }
      ],
      <font color="#0000dd">"Key": "booking"</font>
    },
    {
      "DownstreamPathTemplate": "/api/passenger",
      "UpstreamPathTemplate": "/api/getpassenger",
      "UpstreamHttpMethod": [ "Get" ],
      "ReRouteIsCaseSensitive": false,
      "DownstreamScheme": "http",
      "DownstreamHostAndPorts": [
        {
          "Host": "localhost",
          "Port": 8002
        }
      ],
      <font color="#0000dd">"Key": "passenger"</font>
    }
  ],
  "GlobalConfiguration": {
    "BaseUrl": "https://localhost:5000"
  },
  "Aggregates": [
    {
      "ReRouteKeys": [
        "booking",
        "passenger"
      ],
      "UpstreamPathTemplate": "/api/getbookingpassengerinfo"
    }
  ]
}
```
需要注意的是：
1. 聚合服务目前只支持返回json
2. 目前只支持Get方式请求下游服务
3. 任何下游的response header并会被丢弃
4. 如果下游服务返回404，聚合服务只是这个key的value为空，它不会返回404

有木有觉得这里的聚合很类似于GraphQL的功能，但实际上在Ocelot中并不打算实现GraphQL的功能，因为毕竟Ocelot的主要职责是实现网关的功能，聚合只是其中的一个feature，GraphQL提供了一个库 <font color="#0000dd">graphql-dotnet </font>，我们可以用它来完成需要的功能，而在Ocelot中实现类似认证，授权等这样它擅长的事情：

```javascript
{
    "ReRoutes": [
        {
            "DownstreamPathTemplate": "/graphql",
            "DownstreamScheme": "http",
            "DownstreamHostAndPorts": [
              {
                "Host": "yourgraphqlhost.com",
                "Port": 80
              }
            ],
            "UpstreamPathTemplate": "/graphql",
            "DelegatingHandlers": [
                "GraphQlDelegatingHandler"
            ]
        }
    ]
  }
```
具体课参考下面官方给出的demo：    
https://github.com/ThreeMammals/Ocelot/tree/develop/samples/OcelotGraphQL 

## 参考资料
http://ocelot.readthedocs.io/en/latest/index.html    
https://www.cnblogs.com/shanyou/p/7787183.html

Polly:  
https://github.com/App-vNext/Polly   

Consul:   
https://github.com/hashicorp/consul

Microsoft Blog:  
https://blogs.msdn.microsoft.com/cesardelatorre/2018/05/15/designing-and-implementing-api-gateways-with-ocelot-in-a-microservices-and-container-based-architecture/
