const connection = new signalR.HubConnectionBuilder()
    .withUrl("/gameHub")
    .withAutomaticReconnect()
    .build();
/*function startConnection() {
    if (connection.state === signalR.HubConnectionState.Disconnected) {
        connection.start()
            .then(() => {
                console.log("SignalR Connected");
                //connection.invoke("NotifyConnected"); // Gửi tín hiệu khi kết nối
            })
            .catch(err => console.error("SignalR Connection Error: ", err));
    }
}*/

connection.onreconnected(() => {
    console.log("SignalR Reconnected");
    joinGameGroup();
});

connection.onclose(() => {
    console.log("SignalR Disconnected");
    startConnection();
});
//create game
async function createGame(quizId, hostId) {
    
    if (connection.state === signalR.HubConnectionState.Connected) {
        try {
            await connection.invoke("CreateGame", parseInt(quizId), hostId);
        } catch (err) {
            console.error("CreateGame Error:", err);
        }
    } else {
        console.error("SignalR not connected yet.");
    }
}

/*function joinGameGroup() {
    const gamePin = new URLSearchParams(window.location.search).get("gamePin");
    if (gamePin && connection.state === signalR.HubConnectionState.Connected) {
        connection.invoke("JoinGame", gamePin, `Player_${currentUserId}`)
            .catch(err => console.error("JoinGame Error: ", err));
    }
}*/

// Khởi động kết nối ngay khi script chạy
