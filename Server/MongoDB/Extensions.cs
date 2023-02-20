using BlazorPatchDemo.Server.Interfaces;
using BlazorPatchDemo.Server.Settings;
using BlazorPatchDemo.Shared.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace BlazorPatchDemo.Server.MongoDB;

public static class Extensions
{
    public static IServiceCollection AddMongo(this IServiceCollection services)
    {
        BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
        BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

        services.AddSingleton(serviceProvider =>
        {
            var configuration = serviceProvider.GetService<IConfiguration>();
            if (configuration is null)
                throw new Exception("Fatal: cannot obtain the Configuration values");
            
            var serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
            if (serviceSettings is null)
                throw new Exception("Fatal: cannot obtain the Service Settings");

            var mongoDbSettings = configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
            if (mongoDbSettings is null)
                throw new Exception("Fatal: cannot obtain the Mongo Settings");
            
            // The following operations will either succeed or throw an exception
            var mongoClient = new MongoClient(mongoDbSettings.ConnectionString);
            return mongoClient.GetDatabase(serviceSettings.ServiceName);
        });

        return services;
    }

    public static IServiceCollection AddMongoRepository<T>(this IServiceCollection services, string collectionName)
        where T : IEntity
    {
        services.AddSingleton<IRepository<T>>(serviceProvider =>
        {
            var database = serviceProvider.GetService<IMongoDatabase>();
            if (database is null)
                throw new Exception("Fatal: cannot obtain the Mongo Database");
            
            return new MongoRepository<T>(database, collectionName);
        });

        return services;
    }
}
