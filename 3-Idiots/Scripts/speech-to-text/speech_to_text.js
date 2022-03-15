function recognize() {


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
                } else { // if you want to interact with the site using the record button

                    if (text.includes("open my YouTube")) {

                        console.log("opening youtube");
                        window.open("https://www.youtube.com/channel/UCdxaLo9ALJgXgOUDURRPGiQ");
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
        document.getElementById('rec-id').style.visibility = 'hidden';
       // window.alert(isChrome);
    }
}