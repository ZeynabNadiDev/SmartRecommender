using SmartRecommender.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRecommender.AI.Interfaces
{
    public interface IAiRecommenderService
    {     /// <summary>
          /// Analyzes the user's input message and returns a list of recommended products.
          /// </summary>
          /// <param name="userMessage">User's query or search text (e.g., "Samsung phone under 30 million").</param>
          /// <returns>A list of recommended products.</returns>
        Task <string> GetRecommendationsAsync(string userMessage);
    }
}
