
$(document).ready(function () {
     var name = $('#hName').val();    
     var chatHub = $.connection.chat;
     loadClient(chatHub);    
     
     //start connect hub
     $.connection.hub.start().done(function () {
          chatHub.server.connect(name);
   
          //$("#join").click(function () {
               
          //     chatHub.server.connect(name);
          //     $('.footer').html("Xin chao : " + name);
               
          //});

          $('#btnSend').click(function () {
               var msg = $('#txtMessage').val();
               
               chatHub.server.message(name, msg);
               $('#txtMessage').val('').focus();
          });
     });
});
function loadClient(chatHub) {

     chatHub.client.message = function (name, msg) {
          $('#contentMsg').append("<li><strong>" + name + "</strong>: " + msg + "</li>");
     }
     chatHub.client.connect = function (name) {
          $('#hName').val(name);
     }
}