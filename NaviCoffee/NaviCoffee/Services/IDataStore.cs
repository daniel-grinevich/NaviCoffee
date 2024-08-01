using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NaviCoffee.Models;

namespace NaviCoffee.Services
{

    /*
        “In the request/response cycle of a web application, an interface like 
        IDataStore is crucial for abstracting the data access layer within the server. 
        By defining a consistent set of data operations (e.g., CRUD operations), 
        it helps standardize interactions with the database, regardless of the underlying 
        storage mechanism. This abstraction allows for cleaner, more maintainable code and 
        supports the use of Data Transfer Objects (DTOs) to efficiently transfer only the 
        necessary data between the server and client, enhancing performance and security. 
        The use of IDataStore ensures that the application adheres to defined protocols 
        for data access, contributing to overall system reliability and scalability.”
    */
    public interface IDataStore<T>
    {
        Task<Order> AddOrderAsync(Order order);
        Task<bool> UpdateItemAsync(T item);
        Task<bool> DeleteItemAsync(int id);
        Task<T> GetItemAsync(int id);        
        Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false);
        Task<bool> AddItemToCartAsync(T item);
    }
}
