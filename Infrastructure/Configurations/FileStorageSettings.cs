namespace Infrastructure.Configurations
{
    public class FileStorageSettings
    {
        public string StorageConnectionString { get; set; } 
        public string ContainerName { get; set; } = "imagens";
        public string FileBasePath { get; set; } 
    }
}
