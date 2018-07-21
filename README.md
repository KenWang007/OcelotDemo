# 创建Demo步骤

1.创建BookingApi： dotnet new -n BookingApi  
2.创建PassengerApi： dotnet new -n PassengerApi  
3.创建ApiGateway： dotnet new -n ApiGateway  
4.添加BookingApi和PassengerApi的实现代码  
5.在ApiGateway项目中用Nuget安装Ocelot依赖包  
6.添加configuration.json的配置文件  
7.配置路由响应规则  
8.启动服务并通过Api网关访问服务  
