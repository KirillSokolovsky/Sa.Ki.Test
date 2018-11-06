var frameSyncObjectName = "";

var eventNames = [
    "focus",

    "reset",
    "submit",

    "compositionstart",
    "compositionupdate",
    "compositionend",

    "resize",
    "scroll",

    "keydown",
    "keypress",
    "keyup",

    "mouseenter",
    "mouseover",
    //"mousemove",
    "mousedown",
    "mouseup",
    "auxclick",
    "click",
    "dblclick",
    "contextmenu",
    "wheel",
    "mouseleave",
    "mouseout",
    "select",
    "pointerlockchange",
    "pointerlockerror",

    "dragstart",
    "drag",
    "dragend",
    "dragenter",
    "dragover",
    "dragleave",
    "drop",

    "broadcast",
    "CheckboxStateChange",
    "hashchange",
    "input",
    "RadioStateChange",
    "readystatechange",
    "ValueChange"
];

var log = console.log;

var frameSyncObject;

function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}

function createModelFromTargetElement(el, scanChildren = true, scanParent = true){
    var obj = {};
    obj.TagName = el.localName;
    if(el.attributes != null && el.attributes.length > 0){
        obj.Attributes = new Array();
        for(var i = 0; i < el.attributes.length; i++){
            var at = el.attributes[i];
            obj.Attributes.push({Name: at.name, Value: at.value});
        }
    }
    if(scanChildren && el.children.length > 0){
        obj.Children = new Array();
        for(var i = 0; i < el.children.length; i++){
            var c = el.children[i];
            obj.Children.push(createModelFromTargetElement(c, false, false));
        }
    }

    var parent = el.parentElement;
    if(scanParent && parent != null){
        obj.Parent = createModelFromTargetElement(parent, false);
    }

    return obj;
}

(async function () {
    await CefSharp.BindObjectAsync({ IgnoreCache: true }, frameSyncObjectName);
    frameSyncObject = this[frameSyncObjectName];
    log("bounded: " + frameSyncObjectName);
})()
    .then(async () => await start())
    .catch((e) => log(e));

async function handleEvent(e){

    if(e.type != "click") return;

    log(e);
    var target = createModelFromTargetElement(e.target);
    await frameSyncObject.handleEvent(e.type, JSON.stringify(target));
}

async function start() {

    var style = document.createElement("style");
    style.type = "text/css";
    style.innerHTML = ".my-enter{ background-color: lightgoldenrodyellow; outline:1px solid blue; }";
    document.getElementsByTagName("head")[0].appendChild(style);

    for(i = 0; i < eventNames.length; i++){
        document.addEventListener(eventNames[i], handleEvent);
    }
};