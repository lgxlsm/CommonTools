using Microsoft.Extensions.Configuration;

public static class ApolloCenterExtension
{
    /// <summary>
    /// Apollo配置中心
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="configFolderName"></param>
    /// <param name="configFileName"></param>
    /// <returns></returns>
    public static IConfigurationBuilder AddApolloCenter(this IConfigurationBuilder builder, string configFolderName = "Configuration", string configFileName = null, string sectionName = ApolloCenterOptions.SectionName)
    {
        if (!string.IsNullOrEmpty(configFileName))
        {
            builder.AddConfigurationFile(configFolderName, configFileName);
        }

        builder.AddApollo(builder.Build().GetSection(sectionName));
        return builder;
    }

}
