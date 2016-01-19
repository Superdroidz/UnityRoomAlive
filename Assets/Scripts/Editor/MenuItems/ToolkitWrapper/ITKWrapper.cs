interface ITKWrapper {
    void StartCameraServer();
    void StartProjectorServer();
    void StopServers();
    bool ServersAreRunning();

    void CreateNewSetup(string filepath);
    void RunCalibration(string filepath);
}
