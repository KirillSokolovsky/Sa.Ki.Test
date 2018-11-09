
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
let hClassName = "saki-highlight";

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

async function handleEvent(e){
    if(e.type != "click") return;

    log(e);
    var target = createModelFromTargetElement(e.target);
    log(JSON.stringify(target));
}

(async function start() {

    var style = document.createElement("style");
    style.type = "text/css";
    style.innerHTML = "." + hClassName + "{ background-color: lightgoldenrodyellow; outline:1px solid blue; }";
    document.getElementsByTagName("head")[0].appendChild(style);

    for(i = 0; i < eventNames.length; i++){
        document.addEventListener(eventNames[i], handleEvent);
    }
})();

function getElementsByXpath(path) {
    return document.evaluate(path, document, null, XPathResult.ORDERED_NODE_SNAPSHOT_TYPE, null);
}
function getElementByXpath(path) {
    return document.evaluate(path, document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null);
}

var hss = new Array();

function clearHighlight(){
    hss.forEach((v, i, a) => v.classList.remove(hClassName));
    hss.length = 0;
}

function highlightByXPath(xpath){

    clearHighlight();

    var els = getElementsByXpath(xpath);
    var ln = els.snapshotLength;

    if(ln == 0) return;

    console.log(els);    

    for(var i = 0; i < ln; i++){
        let el = els.snapshotItem(i);
        hss.push(el);
        el.classList.add(hClassName);
    }
}


