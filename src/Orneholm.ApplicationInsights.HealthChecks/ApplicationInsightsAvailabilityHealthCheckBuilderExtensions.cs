﻿using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Orneholm.ApplicationInsights.HealthChecks
{
    public static class ApplicationInsightsAvailabilityHealthCheckBuilderExtensions
    {
        public static IHealthChecksBuilder AddApplicationInsightsAggregatedAvailabilityPublisher(this IHealthChecksBuilder builder)
        {
            return AddApplicationInsightsAggregatedAvailabilityPublisher(builder, null);
        }

        public static IHealthChecksBuilder AddApplicationInsightsAggregatedAvailabilityPublisher(this IHealthChecksBuilder builder, Action<ApplicationInsightsAggregatedAvailibilityPublisherOptions> options)
        {
            if (options != null)
            {
                builder.Services.Configure(options);
            }

            builder.Services.AddSingleton<IHealthCheckPublisher, ApplicationInsightsAggregatedAvailabilityPublisher>();

            return builder;
        }
    }
}
