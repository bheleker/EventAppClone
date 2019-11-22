"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = msg;
    var encodedUser = user;
    var li = document.createElement("p");
    var lk = document.createElement("p");
    li.textContent = encodedMsg;
    lk.textContent = encodedUser;
    document.getElementById("messagesList").appendChild(li);
    document.getElementById("usersList").appendChild(lk);
});

connection.start().then(function(){
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

function logMe(msg) {
    console.log(msg);
}

document.getElementById("sendButton").addEventListener("click", function (event) {
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", message).catch(function (err) {
        return console.error(err.toString());
    });
    const path = window.location.pathname;
    const activityId = path.substring(path.lastIndexOf('/') + 1);
    console.log(activityId);
    console.log(message);
    $.ajax({
        url: "http://localhost:5000/postmessage",
        type: 'POST',
        data: {
            message,
            activityId,
        },
        success: logMe,
        error: function(_, e) {
            console.log(e);
        }
    });
    event.preventDefault();
});
















OLD:





"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = msg;
    var li = document.createElement("p");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});

connection.start().then(function(){
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

function logMe(msg) {
    console.log(msg);
}

document.getElementById("sendButton").addEventListener("click", function (event) {
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", message).catch(function (err) {
        return console.error(err.toString());
    });
    const path = window.location.pathname;
    const activityId = path.substring(path.lastIndexOf('/') + 1);
    console.log(activityId);
    console.log(message);
    $.ajax({
        url: "http://localhost:5000/postmessage",
        type: 'POST',
        data: {
            message,
            activityId,
        },
        success: logMe,
        error: function(_, e) {
            console.log(e);
        }
    });
    event.preventDefault();
});




sample:

"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + " says " + msg;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});

connection.start().then(function(){
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});