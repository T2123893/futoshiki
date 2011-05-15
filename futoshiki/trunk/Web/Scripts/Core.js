/**
 * $Id$
 *
 * Coursework – Futoshiki.Web
 *
 * This file is the result of my own work. Any contributions to the work by 
 * third parties, other than tutors, are stated clearly below this declaration. 
 * Should this statement prove to be untrue I recognise the right and duty of 
 * the Board of Examiners to take appropriate action in line with the university's 
 * regulations on assessment. 
 */


document.onkeydown = onKeyPressed;

var currentCheckedCell;
var rowSize;
var HG = "＞";
var HL = "＜";
var VG = "∨";
var VL = "∧";
var f = {};

window.onload = function() {
    rowSize = FutoshikiTable.rows.length;
    preprocess();
}

function hidMsg() {
    MsgDiv.style.display = "none";
}

function recoverColorOfSign(row, col) {    
    
    var left = $(row + "_" + (col-1));
    if (left &&  left.attributes.ishs && left.innerText != "") {
        var b4Left = $(row + "_" + (col-2));
        if (b4Left && b4Left.attributes.isnum && b4Left.innerText != "") {
            var val = currentCheckedCell.innerText;
            var isG = left.innerText == HG && b4Left.innerText > val;
            var isL = left.innerText == HL && b4Left.innerText < val;
            if (isG || isL || val=="") { left.style.color = "#000000"; }
            else { left.style.color = "red"; }
        } 
    } 
    var right = $(row + "_" + (col+1));
    if (right && right.attributes.ishs && right.innerText != "") {
        var afterRight = $(row + "_" + (col+2));
        if (afterRight && afterRight.attributes.isnum && afterRight.innerText != "") {
            var val = currentCheckedCell.innerText;
            var isG = right.innerText == HG && val > afterRight.innerText;
            var isL = right.innerText == HL && val < afterRight.innerText;
            if (isG || isL || val=="") { right.style.color = "#000000"; }
            else { right.style.color = "red"; }
        }    
    }
    
    var up = $((row-1) + "_" + col);
    if (up && up.attributes.isvs && up.innerText != "") {
        var b4Up = $((row-2) + "_" + col);
        if (b4Up && b4Up.attributes.isnum && b4Up.innerText != "") {
            var val = currentCheckedCell.innerText;
            var isG = up.innerText == VG && b4Up.innerText > val;
            var isL = up.innerText == VL && b4Up.innerText < val;
            if (isG || isL || val=="") { up.style.color = "#000000"; }
            else { up.style.color = "red"; }
        }
    } 
    
    var down = $((row+1) + "_" + col);
    if (down && down.attributes.isvs && down.innerText != "") {
        var afterDown = $((row+2) + "_" + col);
        if (afterDown && afterDown.attributes.isnum && afterDown.innerText != "") {
            var val = currentCheckedCell.innerText;
            var isG = down.innerText == VG && val > afterDown.innerText;
            var isL = down.innerText == VL && val < afterDown.innerText;
            if (isG || isL || val=="") { down.style.color = "#000000"; }
            else {down.style.color = "red"; }
        }
    }
                
}

function checkInequalities(cell) {
    if (cell && cell.innerText != "" && cell.style.color != "red") {
        var h = cell.attributes.ishs ? true : false;
        var greater = h ? HG : VG;
        var lesser = h ? HL : VL;
        var row = parseInt(cell.attributes.row.nodeValue);    
        var col = parseInt(cell.attributes.col.nodeValue);        
        var left = $((h ? row : row-1) + "_" + (h ? (col-1) : col));
        var right = $((h ? row : row+1) + "_" + (h ? (col+1) : col));        
        if (left && right && left.innerText != "" && right.innerText != "") {
            if (cell.innerText == greater && left.innerText <= right.innerText) {
                cell.style.color = "red";
            } else if (cell.innerText == lesser && left.innerText >= right.innerText) {
                cell.style.color = "red";
            }
        }
        
    }        
}

function checkRepeatDigits(cell, noRed) {

    if (cell && cell.innerText != "" && cell.style.color != "red") {
        var row = row = parseInt(cell.attributes.row.nodeValue);    
        var col = col = parseInt(cell.attributes.col.nodeValue);
        
        for (var c = -2; c < rowSize-2; c+=2) {        
            var o = next(row,c);
            if (c+2 != col && cell.innerText == o.innerText) {
                cell.style.color = o.style.color = "red";
            }        
        }

        for (var r = -2;  r < rowSize-2; r+=2) {        
            var o = down(r, col);
            if (r+2 != row && cell.innerText == o.innerText) {
                if (!noRed) {
                    cell.style.color = o.style.color = "red";
                }
            }
        }    
    }
}

function checkAll() {

    if (!$("ChkCorrectness").checked) { 
        recoverColorAll();
        return; 
    }
    
    var cell;
    for (var row = 0; row < rowSize; row++) {
        for (var col = 0; col < rowSize; col++) {
            cell = $(row + "_" + col); 
            if (cell.innerText == "") {           
            } else if (cell.attributes.isnum) {
                checkRepeatDigits(cell); 
            } else if (cell.attributes.ishs || cell.attributes.isvs) {                
                checkInequalities(cell);                 
            }                     
        }
    }
}

function chkRpt4RcvColor(isChkRow, repeat, row, col) {

    var rs = { "repeat":0, "cell":null };
    
    for (var i = -2; i < rowSize-2; i+=2) {        
        var o = isChkRow ? next(row,i) : down(i,col);
        var n = isChkRow ? col : row;
        if (i+2 != n && o.style.color == "red" && o.innerText == repeat) {
            if (++rs.repeat > 1) { 
                rs.repeat = 2;
                return rs; 
            }
            rs.cell = o;
        }      
    }
    
    return rs;
}

function recoverColor(isRow, repeat, row, col) {

    var rs = chkRpt4RcvColor(isRow, repeat, row, col);
    
    if (rs.repeat == 1 && rs.cell) {
        var r = parseInt(rs.cell.attributes.row.nodeValue);
        var c = parseInt(rs.cell.attributes.col.nodeValue);
        var rs1 = chkRpt4RcvColor(!isRow, repeat, r, c);
        if (rs1.repeat == 0) {
            rs.cell.style.color = "#000000";
        } 
    }
    
    if (isRow) { recoverColor(false, repeat, row, col); }
}

function recoverColorOne(repeat,row,col) {

    var colRepeated = 0;
    var repeatCell = null;
    
    for (var c = -2; c < rowSize-2; c+=2) {        
        var o = next(row,c);
        if (c+2 != col && o.style.color == "red" && o.innerText == repeat) {
            if (++colRepeated > 1) { break; }
            repeatCell = o;
        }      
    }
    if (colRepeated == 1 && repeatCell) {        
        repeatCell.style.color = "#000000";    
    }
    
    var rowRepeated = 0;
    for (var r = -2;  r < rowSize-2; r+=2) {        
        var oDown = down(r, col);
        if (r+2 != row && oDown.style.color == "red" && oDown.innerText == repeat) {
            if (++rowRepeated > 1) { break; }
            repeatCell = oDown;
        }
    }              
    if (rowRepeated == 1 && repeatCell) {
        repeatCell.style.color = "#000000";
    }
    
}

function recoverColorAll() {

    for (var r = 0; r < rowSize; r++) {
        for (var c = 0; c < rowSize; c++) {
            cell = $(r + "_" + c);
            if (cell && cell.innerText != "" && cell.style.color == "red") {
                cell.style.color = "#000000";
            }
        }
    }

}

function up(r, c) {

    var previousId = (r-2) + "_" + c;
    while (!$(previousId)) {
        if (c-2 >= 0) {
            c -= 2;
            r = rowSize -1;
        } else {
            r = c = rowSize - 1;
        }
        previousId = r + "_" + c;       
    }
    return $(previousId);
}

function prev(r, c) {

    var previousId = r + "_" + (c-2);
    while (!$(previousId)) {
        if (r-2 >= 0) {
            r -= 2;
            c = rowSize - 1;                        
        } else {
            r = c = rowSize - 1;
        }
        previousId = r + "_" + c;
    }    
    return $(previousId);
}

function next(r, c) {

    var previousId = r + "_" + (c+2);
    while (!$(previousId)) {
        if (r+2 <= rowSize) {
            r += 2;
            c = 0;    
        } else {
            r = c = 0;
        }
        previousId = r + "_" + c;
    }   
    return $(previousId);     
}

function down(r, c) {

    var previousId = (r+2) + "_" + c;
    while (!$(previousId)) {
        if (c+2 <= rowSize) {
            c += 2;
            r = 0;
        } else {
            r = c = 0;
        }
        previousId = r + "_" + c;    
    } 
    return $(previousId);
}

function onKeyPressed() {
    
    key = event.keyCode;
    var isRefreshKey = key == 116;       
     
    var isArrow = key >= 37 && key <= 40;
    var isNum = key >= 48 && key <= 57;    
    if (!currentCheckedCell) { 
        return false; 
    }
    

    var v = String.fromCharCode(key);
    var limit = (rowSize+2)/2;
    if (!isNaN(v)) { 
        v = parseInt(v); 
        if (v > limit) { return false; }
    }    
    if (!isArrow && !isNum && !isRefreshKey) { return false; }
  
  
    var r = parseInt(currentCheckedCell.attributes.row.nodeValue);
    var c = parseInt(currentCheckedCell.attributes.col.nodeValue);
    var isWritable = currentCheckedCell.attributes.iswritable ? true : false;
        
    if ((key == 48 || v == currentCheckedCell.innerText) && isWritable) {
        var bac = currentCheckedCell.innerText;
       // if (bac == v) { return false; }
        currentCheckedCell.innerText = "";
        updt(r,c,"");
        currentCheckedCell.style.color="#000000";
        if ($("ChkCorrectness").checked) { 
            recoverColor(true, bac, r, c);
            recoverColorOfSign(r,c);
        }         
        
    } else if (isNum && currentCheckedCell.innerText == "" ){
        updt(r,c,v);
        currentCheckedCell.innerText = v;    
      //  currentCheckedCell.isSetByClient = true;
        if ($("ChkCorrectness").checked) { 
            checkRepeatDigits(currentCheckedCell); 
            recoverColorOfSign(r,c);
        }        
    } else if (isNum && currentCheckedCell.innertText != "" && isWritable) {
        var bac = currentCheckedCell.innerText;
        if (bac == v) { return false; }
        currentCheckedCell.innerText = v;
        updt(r,c,v);
        currentCheckedCell.style.color = "#000000";
        if ($("ChkCorrectness").checked ) {
            checkRepeatDigits(currentCheckedCell);
            recoverColor(true, bac, r, c);
            checkInequalities(currentCheckedCell);
            recoverColorOfSign(r, c);
        }        
    } else {
        //updt(r,c,v);
    }
          

    switch (key) {
        case 37:
            onCellClick(prev(r,c));
            break;
        case 38:
            onCellClick(up(r,c));
            break;
        case 39:
            onCellClick(next(r, c));
            break;
        case 40:
            onCellClick(down(r, c));
            break;            
        default:
            break;
    }

}

function onCellClick(c) { 

    if (currentCheckedCell && currentCheckedCell.id != c.id) {
        currentCheckedCell.className = "number";
        currentCheckedCell = c;
    }
    
    if (c.className == "number") {
        c.className = "numberChosen";
        currentCheckedCell = c;
    } else {
        c.className = "number";
        currentCheckedCell = null;
    }
} 


function isFinished() {
    
    var f = true;
    var count = 0;

    for (var r = 0; r < rowSize; r++) {
        for (var c = 0; c < rowSize; c++) {
            cell = $(r + "_" + c);
            if (cell.attributes.isnum) {
                var v = cell.innerText != "";
                if (!v) {
                    count++;
                    f &= v;
                }
            }
        }
    }
    
    if (!f) {
        if (!confirm("There are " + count + " unfinished cells, are you sure you want to carry on?")) {
            return {"f":false,"count":count};   
        }  
    }
    //return true;
    return {"f":true,"count":count};        
}

function setHidData() {

    var status = FutoshikiTable.attributes.status.nodeValue;

    // check the grid is finished or not   
    var o = isFinished();  
    if (!o.f) {
        return false;
    } else if (0 == o.count) {
        // to tell server side, this game is finished but stil need double check.
        status = -1;
    }

    var scale = (rowSize+1) / 2;    
    var fid = FutoshikiTable.attributes.fid.nodeValue;
    var str = "<Futoshiki Id=\"" + fid + "\" Scale=\"" + scale + "\" Status=\"" + status + "\">";
    str += "<Cells>";
    
    for (var i = 0; i < f.Length; i++) {
        str += "<Cell Row=\"" + f[i].row + "\" Col=\"" + f[i].col; 
        str += "\" IsWritable=\"" + f[i].isWritable + "\">";
        str += f[i].val + "</Cell>";    
    }
    str += "</Cells></Futoshiki>";     
    $("HidData").value = str;
    return true;
}

function checkSolution(){
    return setHidData();  
}

function showSolution() {
    var o = isFinished();
    if (!o.f) {
        return false;
    }
    $("HidData").value = FutoshikiTable.attributes.fid.nodeValue;
}

function resetGrid() {
    for (var r = 0; r < rowSize; r++) {
        for (var c = 0; c < rowSize; c++) {
            cell = $(r + "_" + c);
            var w = cell.attributes.iswritable ? true : false;
            if (cell.attributes.isnum && w) {
                cell.innerText = "";
            }            
            updt(r,c,cell.innerText,w);
        }
    }
} 


function preprocess() {

    for (var r = 0; r < rowSize; r++) {
        for (var c = 0; c < rowSize; c++) {
            cell = $(r + "_" + c);
            var w = cell.attributes.iswritable ? true : false;
            updt(r,c,cell.innerText,w);
        }
    }
    f.Length = rowSize*rowSize;
}   

function updt(r,c,v,w) {

    var i = r*rowSize+c; 
    if (!f[i]) {
        var cell = {};
        cell.row = r;
        cell.col = c;
        cell.val = v;
        cell.isWritable = w;
        f[i] = cell;
    } else {
        f[i].val = v;
    }
}

function $(e) {
    return document.getElementById(e);
}