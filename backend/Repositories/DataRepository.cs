using MongoDB.Driver;
using Microsoft.Extensions.Options;
using dotNet_bakery.Models;
using MongoDB.Bson;

namespace dotNet_bakery.Repo;
public class DataRepository
{
    private readonly IMongoCollection<DataModel> _dataModels;

    public DataRepository(IOptions<MongoDBSettings> mongoDBSetting)
    {
        MongoClient client = new MongoClient(mongoDBSetting.Value.ConnectionURI);
        IMongoDatabase database = client.GetDatabase(mongoDBSetting.Value.DatabaseName);
        _dataModels = database.GetCollection<DataModel>(mongoDBSetting.Value.CollectionName);

        //var database = client.GetDatabase("testdb");
        //_dataModels = database.GetCollection<DataModel>("datamodels");
    }

    public async Task CreateAsync(DataModel dataModel){
        await _dataModels.InsertOneAsync(dataModel);
        return;
    }

    public async Task<List<DataModel>> GetAsync(){
        return await _dataModels.Find(new BsonDocument()).ToListAsync();
    }

    public async Task DeleteAsync(string id){
        FilterDefinition<DataModel> filter = Builders<DataModel>.Filter.Eq("id", id);
        await _dataModels.DeleteOneAsync(filter);
        return;
    }

/*
    public List<DataModel> Get() =>
        _dataModels.Find(dataModel => true).ToList();

    public DataModel Get(string id) =>
        _dataModels.Find<DataModel>(dataModel => dataModel.Id == id).FirstOrDefault();

    public DataModel Create(DataModel dataModel)
    {
        _dataModels.InsertOne(dataModel);
        return dataModel;
    }

    public void Update(string id, DataModel dataModelIn) =>
        _dataModels.ReplaceOne(dataModel => dataModel.Id == id, dataModelIn);

    public void Remove(DataModel dataModelIn) =>
        _dataModels.DeleteOne(dataModel => dataModel.Id == dataModelIn.Id);

    public void Remove(string id) => 
        _dataModels.DeleteOne(dataModel => dataModel.Id == id);
        */
}