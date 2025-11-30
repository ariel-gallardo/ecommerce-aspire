using Ecommerce;
using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);
var cfg = builder.Configuration;

var redisDataMount = cfg["Parameters:Redis:DataMount"];
var rabbitDataMount = cfg["Parameters:RabbitMQ:DataMount"];
var mySQLDataMountBase = cfg["Parameters:MySQL:DataMount"];

var redisPass = builder.AddParameterFromConfiguration("RedisPassword", "Parameters:Redis:Password");
var rabbitUser = builder.AddParameterFromConfiguration("RabbitUser", "Parameters:RabbitMQ:Username");
var rabbitPass = builder.AddParameterFromConfiguration("RabbitPassword", "Parameters:RabbitMQ:Password");
var mySQLPass = builder.AddParameterFromConfiguration("RabbitPassword", "Parameters:MySQL:Password");

var cache = builder.AddRedis("cache", password: redisPass);
var rabbitMQ = builder.AddRabbitMQ("rabbit", rabbitUser, rabbitPass);

var isTesting = builder.Environment.IsEnvironment("Testing");

    var basePath = cfg["Parameters:DatabaseTestingPath"];
    var cartDbFile = "Cart.sqlite";
    var inventoryDbFile = "Inventory.sqlite";
    var invoiceDbFile = "Invoice.sqlite";
    var logsDbFile = "Logs.sqlite";
    var notificationDbFile = "Notification.sqlite";
    var orderDbFile = "Order.sqlite";
    var paymentDbFile = "Payment.sqlite";
    var productDbFile = "Product.sqlite";
    var securityDbFile = "Security.sqlite";
    var shippingDbFile = "Shipping.sqlite";

    var cartDbTesting = isTesting ? builder.AddSqlite("CartDb",basePath, cartDbFile).WithSqliteWeb() : null;
    var inventoryDbTesting = isTesting ? builder.AddSqlite("InventoryDb", basePath, inventoryDbFile).WithSqliteWeb() : null;
    var invoiceDbTesting = isTesting ? builder.AddSqlite("InvoiceDb", basePath, invoiceDbFile).WithSqliteWeb() : null;
    var logsDbTesting = isTesting ? builder.AddSqlite("LogsDb", basePath, logsDbFile).WithSqliteWeb() : null;
    var notificationDbTesting = isTesting ? builder.AddSqlite("NotificationDb", basePath, notificationDbFile).WithSqliteWeb() : null;
    var orderDbTesting = isTesting ? builder.AddSqlite("OrderDb", basePath, orderDbFile).WithSqliteWeb() : null;
    var paymentDbTesting = isTesting ? builder.AddSqlite("PaymentDb", basePath, paymentDbFile).WithSqliteWeb() : null;
    var productDbTesting = isTesting ? builder.AddSqlite("ProductDb", basePath, productDbFile).WithSqliteWeb() : null;
    var securityDbTesting = isTesting ? builder.AddSqlite("SecurityDb", basePath, securityDbFile).WithSqliteWeb() : null;
    var shippingDbTesting = isTesting ? builder.AddSqlite("SecurityDb", basePath, shippingDbFile).WithSqliteWeb() : null;

    var cartDb = !isTesting ? builder.AddMySql("Cartdb", mySQLPass).WithDataBindMount(Path.Join(mySQLDataMountBase, "cart")).WithPhpMyAdmin() : null;
    var inventoryDb = !isTesting ? builder.AddMySql("InventoryDb", mySQLPass).WithDataBindMount(Path.Join(mySQLDataMountBase, "inventory")).WithPhpMyAdmin() : null;
    var invoiceDb = !isTesting ? builder.AddMySql("InvoiceDb", mySQLPass).WithDataBindMount(Path.Join(mySQLDataMountBase, "invoice")).WithPhpMyAdmin() : null;
    var logsDb = !isTesting ? builder.AddMySql("LogsDb", mySQLPass).WithDataBindMount(Path.Join(mySQLDataMountBase, "logs")).WithPhpMyAdmin() : null;
    var notificationDb = !isTesting ? builder.AddMySql("NotificationDb", mySQLPass).WithDataBindMount(Path.Join(mySQLDataMountBase, "notification")).WithPhpMyAdmin() : null;
    var orderDb = !isTesting ? builder.AddMySql("OrderDb", mySQLPass).WithDataBindMount(Path.Join(mySQLDataMountBase, "order")).WithPhpMyAdmin() : null;
    var paymentDb = !isTesting ? builder.AddMySql("PaymentDb", mySQLPass).WithDataBindMount(Path.Join(mySQLDataMountBase, "payment")).WithPhpMyAdmin() : null;
    var productDb = !isTesting ? builder.AddMySql("ProductDb", mySQLPass).WithDataBindMount(Path.Join(mySQLDataMountBase, "product")).WithPhpMyAdmin() : null;
    var securityDb = !isTesting ? builder.AddMySql("SecurityDb", mySQLPass).WithDataBindMount(Path.Join(mySQLDataMountBase, "security")).WithPhpMyAdmin() : null;
    var shippingDb = !isTesting ? builder.AddMySql("ShippingDb", mySQLPass).WithDataBindMount(Path.Join(mySQLDataMountBase, "shipping")).WithPhpMyAdmin() : null;


var cartApi = builder.AddProject<Projects.Cart_API>("cart")
    .WithOpenApi()
    .WithAppSettingsEnvironments(cfg)
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(isTesting ? cartDbTesting : cartDb)
    .WithReference(rabbitMQ)
    .WithReference(cache)
    .WaitFor(rabbitMQ)
    .WaitFor(cache)
    .WaitFor(isTesting ? cartDbTesting : cartDb);
var inventoryApi = builder.AddProject<Projects.Inventory_API>("inventory")
    .WithOpenApi()
    .WithAppSettingsEnvironments(cfg)
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(isTesting ? inventoryDbTesting : inventoryDb)
    .WithReference(rabbitMQ)
    .WithReference(cache)
    .WaitFor(rabbitMQ)
    .WaitFor(cache)
    .WaitFor(isTesting ? inventoryDbTesting : inventoryDb);
var invoiceApi = builder.AddProject<Projects.Invoice_API>("invoice")
    .WithOpenApi()
    .WithAppSettingsEnvironments(cfg)
    .WithReference(isTesting ? invoiceDbTesting : invoiceDb)
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(rabbitMQ)
    .WithReference(cache)
    .WaitFor(rabbitMQ)
    .WaitFor(cache)
    .WaitFor(isTesting ? invoiceDbTesting : invoiceDb);
var logsApi = builder.AddProject<Projects.Logs_API>("logs")
    .WithOpenApi()
    .WithAppSettingsEnvironments(cfg)
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(isTesting ? logsDbTesting : logsDb)
    .WithReference(rabbitMQ)
    .WithReference(cache)
    .WaitFor(rabbitMQ)
    .WaitFor(cache)
    .WaitFor(isTesting ? logsDbTesting : logsDb);
var notificationApi = builder.AddProject<Projects.Notification_API>("notification")
    .WithOpenApi()
    .WithAppSettingsEnvironments(cfg)
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(isTesting ? notificationDbTesting : notificationDb)
    .WithReference(rabbitMQ)
    .WithReference(cache)
    .WaitFor(rabbitMQ)
    .WaitFor(cache)
    .WaitFor(isTesting ? notificationDbTesting : notificationDb);
var orderApi = builder.AddProject<Projects.Order_API>("order")
    .WithOpenApi()
    .WithAppSettingsEnvironments(cfg)
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(isTesting ? orderDbTesting : orderDb)
    .WithReference(rabbitMQ)
    .WithReference(cache)
    .WaitFor(rabbitMQ)
    .WaitFor(cache)
    .WaitFor(isTesting ? orderDbTesting : orderDb);
var paymentApi = builder.AddProject<Projects.Payment_API>("payment")
    .WithOpenApi()
    .WithAppSettingsEnvironments(cfg)
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(isTesting ? paymentDbTesting : paymentDb)
    .WithReference(rabbitMQ)
    .WithReference(cache)
    .WaitFor(rabbitMQ)
    .WaitFor(cache)
    .WaitFor(isTesting ? paymentDbTesting : paymentDb);
var productApi = builder.AddProject<Projects.Product_API>("product")
    .WithOpenApi()
    .WithAppSettingsEnvironments(cfg)
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(isTesting ? productDbTesting : productDb)
    .WithReference(rabbitMQ)
    .WithReference(cache)
    .WaitFor(rabbitMQ)
    .WaitFor(cache)
    .WaitFor(isTesting ? productDbTesting : productDb);
var securityApi = builder.AddProject<Projects.Security_API>("security")
    .WithOpenApi()
    .WithAppSettingsEnvironments(cfg)
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(isTesting ? securityDbTesting : securityDb)
    .WithReference(rabbitMQ)
    .WithReference(cache)
    .WaitFor(rabbitMQ)
    .WaitFor(cache)
    .WaitFor(isTesting ? securityDbTesting : securityDb);
var shippingApi = builder.AddProject<Projects.Shipping_API>("shipping")
    .WithOpenApi()
    .WithAppSettingsEnvironments(cfg)
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(isTesting ? shippingDbTesting : shippingDb)
    .WithReference(rabbitMQ)
    .WithReference(cache)
    .WaitFor(rabbitMQ)
    .WaitFor(cache)
    .WaitFor(isTesting ? shippingDbTesting : shippingDb);

var apiGateway = builder.AddProject<Projects.ApiGateway>("api-gateway")
    .WithExternalHttpEndpoints()
    .WithReference(cartApi)
    .WithReference(inventoryApi)
    .WithReference(invoiceApi)
    .WithReference(logsApi)
    .WithReference(notificationApi)
    .WithReference(orderApi)
    .WithReference(paymentApi)
    .WithReference(productApi)
    .WithReference(securityApi)
    .WithReference(shippingApi);

builder.Build().Run();
