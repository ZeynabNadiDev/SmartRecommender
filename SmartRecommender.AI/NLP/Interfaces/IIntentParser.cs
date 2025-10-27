using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartRecommender.Domain.AI.Models;

namespace SmartRecommender.AI.NLP.Interfaces
{
    public interface IIntentParser
    {
        Task<FeatureVector> ParseAsync(string userInput);
    }
}
