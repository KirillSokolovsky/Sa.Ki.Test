var frameSyncObjectName = "";

document.addEventListener("click", (e) =>
    console.log(e.type + " " + frameSyncObjectName));

var frameSyncObject;

function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
  }

(async function () {
    await CefSharp.BindObjectAsync({ IgnoreCache: true }, frameSyncObjectName);
    frameSyncObject = this[frameSyncObjectName];
    console.log("bounded: " + frameSyncObjectName);
})()
.then(async () => await start())
.catch((e) => console.log(e));

async function start(isHealth = false){

    let from = isHealth ? "Helthcheck" : "Hello";
    try{
        let s = await frameSyncObject.test();
        console.log(from + " from: " + s);
    }
    catch(err){
        console.log("Error " + from + ": " + err);
    }

    if(isHealth) return;

    console.log("Start sleep: " + frameSyncObjectName);
    sleep(5000)
        .then(async () => await start(true))
        .catch((e)=> console.log("Error Helthcheck sleep: " + e));
};