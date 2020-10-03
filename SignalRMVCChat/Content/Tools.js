/*  <div class="alert alert-danger" id="err" style="display: none"></div>
        <div class="alert alert-success" id="msg" style="display: none"></div>
*/
function initMsssages(cls) {
    
    var msg = document.getElementById('msg');
    if (!msg) {
        
        let div=document.createElement('div');
        div.innerHTML=`
        <div class="alert alert-success" id="msg" style="display: none"></div>
`;

        if ($ &&  $(cls ? cls:'.gapContent'))
            $(cls ? cls: '.gapContent')[0].append(div);
        else{
            getDoc().append(div);
        }
    }
    var err = document.getElementById('err');
    if (!err) {
        let div=document.createElement('div');
        div.innerHTML=`
           <div class="alert alert-danger" id="err" style="display: none"></div>
`;
        
        if ($ && $(cls ? cls:'.gapContent'))
        $(cls ? cls:'.gapContent')[0].append(div);
        else{
            getDoc().append(div);
        }
    }
}

function clearMsg() {
    $('#msg').hide();
    $('#err').hide();

    }

function showError(text,cls) {
    initMsssages(cls);
   
    $('#msg').hide();
    $('#err').show();
    $('#err').html(text);

    setTimeout(function() {
        clearMsg();
    },5000)
}

function showSuccess(text,cls) {
    initMsssages(cls);
    $('#err').hide();
    $('#msg').show();
    $('#msg').text(text);


    setTimeout(function () {
        clearMsg();
    }, 7000)
}

function ajaxCallFill(url, target, body) {

    $.ajax({
        url: url,
        data: body,
        method: 'post',
        success: function (res) {
            $(target).html(res);

            $('#submitButton').show();
        },
        error: function (err) {

            console.error(err);
            showError('خطایی رخ داد' + err.responseText);
        }

    })
}