var letters = 'ABCÇDEFGÐHIÝJKLMNOÖPQRSÞTUÜVWXYZabcçdefgðhýijklmnoöpqrsþtuüvwxyz'
var numbers = '01234567890'
var money = '01234567890.,'
var signs = ',.:;@-\''
var mathsigns = '+-=()*/'
var custom = '<>#$%&?¿'

function alphakont(e, allow) {
    var k;
    k = document.all ? parseInt(e.keyCode) : parseInt(e.which);
    return (allow.indexOf(String.fromCharCode(k)) != -1);
}

//<input type="text" onkeypress="return alpha(event,numbers)" />
//<input type="text" onkeypress="return alpha(event,letters)" />
//<input type="text" onkeypress="return alpha(event,numbers+letters+signs)" />
