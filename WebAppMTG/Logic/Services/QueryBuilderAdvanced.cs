using System;
using System.Collections.Generic;
using System.Text;
using WebAppMTGLogic.Models;

namespace WebAppMTGLogic.Services
{
    public class QueryBuilderAdvanced
    {
        public string Build(AdvancedSearchModel search)
        {
            var parts = new List<string>();

            if (!string.IsNullOrWhiteSpace(search.Name))
                parts.Add($"name:{search.Name}");

            if (!string.IsNullOrWhiteSpace(search.OracleText))
                parts.Add($"oracle:{search.OracleText}");

            if (!string.IsNullOrWhiteSpace(search.TypeLine))
                parts.Add($"type:{search.TypeLine}");

            if (!string.IsNullOrWhiteSpace(search.ManaValue))
                parts.Add($"mv:{search.ManaValue}");

            if (!string.IsNullOrWhiteSpace(search.Color))
                parts.Add($"color={search.Color}");

            if (!string.IsNullOrWhiteSpace(search.Format))
                parts.Add($"format:{search.Format}");

            return string.Join(" ", parts);
        }
    }
}
