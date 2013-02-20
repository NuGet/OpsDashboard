using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuGetGallery.Dashboard.Configuration
{
    /// <summary>
    /// Service for reading Secret, key-value settings
    /// </summary>
    public interface ISecretsService
    {
        string GetSetting(string key);
    }
}
