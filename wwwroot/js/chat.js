"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = msg;
    var encodedUser = user+": ";
    var ms = document.createElement("span");
    var us = document.createElement("span");
    var time = document.createElement("span");//append this to flexcontainer
    var flexContainer = document.createElement("div");
    flexContainer.classList.add("flexContainer")
    time.classList.add("messageDetail");
    ms.setAttribute("class", "chatMessage");
    us.setAttribute("style", "font-weight:bold; font-style:oblique");
    time.innerText = "-Now";
    var text = document.createTextNode(encodedMsg);
    us.textContent = encodedUser;
    ms.appendChild(us);
    ms.appendChild(text);
    flexContainer.appendChild(ms);
    flexContainer.appendChild(time);
    document.getElementById("usersList").appendChild(flexContainer);
    var messageFlex = document.querySelector('.messageFlex');
    messageFlex.scrollTop = messageFlex.scrollHeight - messageFlex.clientHeight;
});

connection.start().then(function(){
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

function logMe(msg) {
    console.log(msg);
}
var messageFlex = document.querySelector('.messageFlex');
messageFlex.scrollTop = messageFlex.scrollHeight - messageFlex.clientHeight;

document.getElementById("messageForm").addEventListener("submit", function (event) {
    var user = document.getElementById("hiddenUser").value;
    var messageInput = document.getElementById("messageInput");
    var message = messageInput.value;
    messageInput.value = "";
    connection.invoke("SendMessage", user, message).catch(function (err) {
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