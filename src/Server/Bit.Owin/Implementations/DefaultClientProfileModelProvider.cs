﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bit.Core.Contracts;
using Bit.Core.Models;
using Bit.Model.DomainModels;
using Bit.Owin.Contracts;
using Bit.Owin.Models;

namespace Bit.Owin.Implementations
{
    public class DefaultClientProfileModelProvider : IClientProfileModelProvider
    {
        private IAppEnvironmentProvider _AppEnvironmentProvider;
        private AppEnvironment _App;

        public virtual IAppEnvironmentProvider AppEnvironmentProvider
        {
            get => _AppEnvironmentProvider;
            set
            {
                _AppEnvironmentProvider = value;
                _App = _AppEnvironmentProvider.GetActiveAppEnvironment();
            }
        }
        
        public virtual IUserSettingProvider UsersSettingsProvider { get; set; }
        public virtual IContentFormatter ContentFormatter { get; set; }

        public virtual ClientProfileModel GetClientProfileModel()
        {
            ClientProfileModel clientProfileMdoel = new ClientProfileModel
            {
                AppVersion = _App.AppInfo.Version,
                DebugMode = _App.DebugMode,
                AppName = _App.AppInfo.Name
            };

            UserSetting userSetting = UsersSettingsProvider?.GetCurrentUserSetting();

            string theme = userSetting?.Theme ?? _App.AppInfo.DefaultTheme;

            string culture = userSetting?.Culture ?? _App.AppInfo.DefaultCulture;

            string desiredTimeZone = userSetting?.DesiredTimeZone ??
                                           _App.AppInfo.DefaultTimeZone;

            string desiredTimeZoneValue = null;

            if (culture == null || string.Equals(culture, "Auto", StringComparison.OrdinalIgnoreCase))
                culture = "EnUs";

            if (desiredTimeZone != null &&
                !string.Equals(desiredTimeZone, "Auto", StringComparison.CurrentCulture))
                desiredTimeZoneValue = desiredTimeZone;

            string appTitle = _App.Cultures.Any() ? _App.Cultures
                .ExtendedSingle($"Finding culture {culture} in environment {_App.Name}", c => c.Name == culture).Values.ExtendedSingle($"Finding AppTitle in culture {culture}", v =>
                      string.Equals(v.Name, "AppTitle", StringComparison.OrdinalIgnoreCase)).Title : string.Empty;

            clientProfileMdoel.AppTitle = appTitle;
            clientProfileMdoel.Culture = culture;
            clientProfileMdoel.DesiredTimeZoneValue = desiredTimeZoneValue;
            clientProfileMdoel.Theme = theme;

            clientProfileMdoel.EnvironmentConfigsJson = ContentFormatter.Serialize(_App
                .Configs.Where(c => c.AccessibleInClientSide == true)
                .Select(c => new { value = c.Value, key = c.Key }));

            clientProfileMdoel.BaseHref = _App.GetHostVirtualPath();

            return clientProfileMdoel;
        }

        public virtual async Task<ClientProfileModel> GetClientProfileModelAsync(CancellationToken cancellationToken)
        {
            ClientProfileModel clientAppProfileModel = new ClientProfileModel
            {
                AppVersion = _App.AppInfo.Version,
                DebugMode = _App.DebugMode,
                AppName = _App.AppInfo.Name
            };

            UserSetting userSetting = UsersSettingsProvider == null ? null : await UsersSettingsProvider.GetCurrentUserSettingAsync(cancellationToken).ConfigureAwait(false);

            string theme = userSetting?.Theme ?? _App.AppInfo.DefaultTheme;

            string culture = userSetting?.Culture ?? _App.AppInfo.DefaultCulture;

            string desiredTimeZone = userSetting?.DesiredTimeZone ??
                                           _App.AppInfo.DefaultTimeZone;

            string desiredTimeZoneValue = null;

            if (culture == null || string.Equals(culture, "Auto", StringComparison.OrdinalIgnoreCase))
                culture = "EnUs";

            if (desiredTimeZone != null &&
                !string.Equals(desiredTimeZone, "Auto", StringComparison.CurrentCulture))
                desiredTimeZoneValue = desiredTimeZone;

            string appTitle = _App.Cultures.Any() ? _App.Cultures
                .ExtendedSingle($"Finding culture {culture} in environment {_App.Name}", c => c.Name == culture).Values.ExtendedSingle($"Finding AppTitle in culture {culture}", v =>
                      string.Equals(v.Name, "AppTitle", StringComparison.OrdinalIgnoreCase)).Title : string.Empty;

            clientAppProfileModel.AppTitle = appTitle;
            clientAppProfileModel.Culture = culture;
            clientAppProfileModel.DesiredTimeZoneValue = desiredTimeZoneValue;
            clientAppProfileModel.Theme = theme;

            clientAppProfileModel.EnvironmentConfigsJson = ContentFormatter.Serialize(_App
                .Configs.Where(c => c.AccessibleInClientSide == true)
                .Select(c => new { value = c.Value, key = c.Key }));

            clientAppProfileModel.BaseHref = _App.GetHostVirtualPath();

            return clientAppProfileModel;
        }
    }
}