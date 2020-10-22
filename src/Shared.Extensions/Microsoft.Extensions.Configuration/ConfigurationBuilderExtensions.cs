﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using System;

namespace GoodToCode.Shared.Extensions
{
    public static class ConfigurationBuilderExtensions
    {
        public static IConfiguration AddAzureAppConfigurationDefault(this ConfigurationBuilder item, string sentinelAppConfigKey)
        {            
            return AddAzureAppConfigurationDefault(item, sentinelAppConfigKey, Environment.GetEnvironmentVariable("AppSettingsConnection"));
        }

        public static IConfiguration AddAzureAppConfigurationDefault(this ConfigurationBuilder item, string sentinelAppConfigKey, string appConfigurationConnection)
        {
            item.AddAzureAppConfiguration(options =>
                            options
                                .Connect(appConfigurationConnection)
                                .ConfigureRefresh(refresh =>
                                {
                                    refresh.Register(sentinelAppConfigKey, refreshAll: true)
                                           .SetCacheExpiration(new TimeSpan(0, 60, 0));
                                })
                                .Select(KeyFilter.Any, LabelFilter.Null)
                                .Select(KeyFilter.Any, Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production")
                        );
            return item.Build();
        }
    }
}
