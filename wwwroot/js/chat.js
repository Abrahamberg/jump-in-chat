
"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

var usrbox = document.getElementById("userInput");

connection.on("ReceiveMessage", function (user, message) {
    if (usrbox.value === user) return; 
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    li.innerHTML = `<span style='color:green'> ${user} : ${message} </span>`;
});

connection.on("ReceivePrivate", function (user, message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    li.innerHTML = `<span style='color:#bf00ff'> ${user} : ${message} </span>`;
});


connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = usrbox.value;
    usrbox.disabled = true;
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    var message = document.getElementById("messageInput").value;
    li.innerHTML = `<span style='color:red'> ${user} : ${message} </span>`;
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});