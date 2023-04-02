namespace SystemModule.Sockets.Event;

public delegate void DSCClientOnReceiveHandler(object sender, DSCClientDataInEventArgs e);

public delegate void ClientOnReceiveHandler(object sender, ClientReceiveDataEventArgs e);