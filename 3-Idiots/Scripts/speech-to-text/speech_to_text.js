
function recognize(isSearch) {


    var isChrome = !!window.chrome && (!!window.chrome.webstore || !!window.chrome.runtime);
    var value = "listening now...";
    if (isChrome) {

        window.SpeechRecognition = window.speechRecognition || window.webkitSpeechRecognition;

        const recognition = new SpeechRecognition();
        recognition.interimResults = true;

        $("#speech_to_text").val(value);

        recognition.addEventListener("result", (e) => {

            const text = Array.from(e.results)
                .map((result) => result[0])
                .map((result) => result.transcript)
                .join("");


            if (e.results[0].isFinal) {
                /// if you want to use the record button to search
                if (isSearch) {
                    value = $("#speech_to_text").val();
                    value = text;
                    $("#speech_to_text").val(value);
                }
                else { 

                    if ((text.includes("type question") || text.includes("type")) && document.getElementById("speech_to_text") != null) {
                        document.getElementById("speech_to_text").focus()
                    }
                    else if (text.includes("record question") && document.getElementById("rec-id") != null) {
                        document.getElementById("rec-id").click()
                    }
                    else {
                        window.open('https://localhost:44331/QandA/Navigate?search=' + text + '&currentUrl=' + window.location.href, '_self');

                    }
                }


            }
        });



        recognition.start();

    } else {
        value = "use chrome if you want to utilize the voice search";
        $("#speech_to_text").val(value);
    }





}


function onloadfunction() {

    var isChrome = !!window.chrome && (!!window.chrome.webstore || !!window.chrome.runtime);

    if (!isChrome) {
        if (document.getElementById('rec-control-id') != null) {
            document.getElementById('rec-control-id').style.visibility = 'hidden';
        }
        if (document.getElementById('rec-id') != null) {
            document.getElementById('rec-id').style.visibility = 'hidden';

        }

       
    }
}