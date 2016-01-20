interface ITKWrapper {
    /*
    * Starts camera server if it is not running already.
    */
    void StartCameraServer();

    /*
    * Starts projector server if it is not running already.
    */
    void StartProjectorServer();

    /*
    * Stops all currently running camera and projector servers.
    */
    void StopServers();

    /*
    * Returns true if either a projector or a camera server is running.
    */
    bool ServersAreRunning();

    /*
    * Creates a new toolkit setup.
    *
    * 'Filepath' indicates location of where the setup should be saved.
    */
    void CreateNewSetup(string filepath);

    /*
    * Runs toolkit calibration operations.
    *
    * Results should be saved to root of the 'filepath' folder.
    */
    void RunCalibration(string filepath);
}
