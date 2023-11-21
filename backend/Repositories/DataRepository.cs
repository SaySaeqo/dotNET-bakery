using MongoDB.Driver;

public class DataRepository
{
    private readonly IMongoCollection<DataModel> _dataModels;

    public DataRepository(IMongoClient client)
    {
        var database = client.GetDatabase("testdb");
        _dataModels = database.GetCollection<DataModel>("datamodels");
    }

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
}