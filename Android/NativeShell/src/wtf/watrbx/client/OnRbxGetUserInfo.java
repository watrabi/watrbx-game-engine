package wtf.watrbx.client;

abstract class OnRbxGetUserInfo {
    public OnRbxGetUserInfo(boolean oldFb) {
        wasFbConnected = oldFb;
    }
    public boolean wasFbConnected;
    public abstract void onResponse();
}
