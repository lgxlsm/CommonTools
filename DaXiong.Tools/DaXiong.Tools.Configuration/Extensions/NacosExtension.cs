using Microsoft.Extensions.Configuration;

public static class NacosExtension
{
    /// <summary>
    /// Apollo配置中心
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="configFolderName"></param>
    /// <param name="configFileName"></param>
    /// <returns></returns>
    public static IConfigurationBuilder AddNacos(this IConfigurationBuilder builder, string configFolderName = "Configuration", string configFileName = null)
    {
        if (!string.IsNullOrEmpty(configFileName))
        {
            builder.AddConfigurationFile(configFolderName, configFileName);
        }

        builder.AddNacosV2Configuration(builder.Build().GetSection(NacosOptions.SectionName));
        return builder;
    }

}
