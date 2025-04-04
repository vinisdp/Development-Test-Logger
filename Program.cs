internal class Program
{
    private static async Task Main(string[] args)
    {
        Logger logger = new Logger("app.log", LogLevel.Info);

        await logger.Info("Aplicação iniciada.");
        await logger.Debug("Este log não será salvo pois o nível mínimo é Info.");
        await logger.Warning("Alerta: possível sobrecarga no sistema.");
        await logger.Error("Erro crítico detectado!");

        try
        {
            int x = 0;
            int y = 10 / x;
        }
        catch (Exception ex)
        {
            await logger.Error(ex);
        }
    }
}
