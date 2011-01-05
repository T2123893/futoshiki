/**
* $Id$
*
* Coursework – Javascript file.
*
* This file is the result of my own work. Any contributions to the work by third parties,
* other than tutors, are stated clearly below this declaration. Should this statement prove to
* be untrue I recognise the right and duty of the Board of Examiners to take appropriate
* action in line with the university's regulations on assessment. 
*/
 


//document.oncontextmenu=new Function("event.returnValue=false;");
//document.onselectstart=new Function("event.returnValue=false;");
document.onkeydown = onKeyPressed;

var currentCheckedCell;
var rowSize;
var HG = "＞";
var HL = "＜";
var VG = "∨";
var VL = "∧";

window.onload = function() {
    rowSize = FutoshikiTable.rows.length;
}

function recoverColorOfSign(row, col) {    
    
    var left = $(row + "_" + (col-1));
    if (left &&  left.attributes.ishs && left.innerText != "") {
        var b4Left = $(row + "_" + (col-2));
        if (b4Left && b4Left.attributes.isnum && b4Left.innerText != "") {
            var isG = left.innerText == HG && b4Left.innerText > currentCheckedCell.innerText;
            var isL = left.innerText == HL && b4Left.innerText < currentCheckedCell.innerText;
            if (isG || isL) { left.style.color = "#000000"; }
            else { left.style.color = "red"; }
        } 
    } 
    var right = $(row + "_" + (col+1));
    if (right && right.attributes.ishs && right.innerText != "") {
        var afterRight = $(row + "_" + (col+2));
        if (afterRight && afterRight.attributes.isnum && afterRight.innerText != "") {
            var isG = right.innerText == HG && currentCheckedCell.innerText > afterRight.innerText;
            var isL = right.innerText == HL && currentCheckedCell.innerText < afterRight.innerText;
            if (isG || isL) { right.style.color = "#000000"; }
            else { right.style.color = "red"; }
        }    
    }
    
    var up = $((row-1) + "_" + col);
    if (up && up.attributes.isvs && up.innerText != "") {
        var b4Up = $((row-2) + "_" + col);
        if (b4Up && b4Up.attributes.isnum && b4Up.innerText != "") {
            var isG = up.innerText == VG && b4Up.innerText > currentCheckedCell.innerText;
            var isL = up.innerText == VL && b4Up.innerText < currentCheckedCell.innerText;
            if (isG || isL) { up.style.color = "#000000"; }
            else { up.style.color = "red"; }
        }
    } 
    
    var down = $((row+1) + "_" + col);
    if (down && down.attributes.isvs && down.innerText != "") {
        var afterDown = $((row+2) + "_" + col);
        if (afterDown && afterDown.attributes.isnum && afterDown.innerText != "") {
            var isG = down.innerText == VG && currentCheckedCell.innerText > afterDown.innerText;
            var isL = down.innerText == VL && currentCheckedCell.innerText < afterDown.innerText;
            if (isG || isL) { down.style.color = "#000000"; }
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
/*
function checkHorizontalInequalities(cell) {
    if (cell && cell.innerText != "" && cell.style.color != "red") {
        var row = row = parseInt(cell.attributes.row.nodeValue);    
        var col = col = parseInt(cell.attributes.col.nodeValue);
        var left = $(row + "_" + (col-1));
        var right = $(row + "_" + (col+1));        
        if (left && right && left.innerText != "" && right.innerText != "") {
            //alert(cell.innerText);      
            if (cell.innerText == "＞" && left.innerText <= right.innerText) {
                cell.style.color = "red";
            } else if (cell.innerText == "＜" && left.innerText >= right.innerText) {
                cell.style.color = "red";
            } 
        }
        
    }        
}

function checkVerticalInequalities(cell) {

    if (cell && cell.innerText != "" && cell.style.color != "red") {
        var row = row = parseInt(cell.attributes.row.nodeValue);    
        var col = col = parseInt(cell.attributes.col.nodeValue);
        var up = $((row-1) + "_" + col);
        var down = $((row+1) + "_" + col);
        if (up && down && up.innerText != "" && down.innerText != "") {
            if (cell.innerText == "∨" && up.innerText <= down.innerText) {
                cell.style.color = "red";
            } else if (cell.innerText == "∧" && up.innerText >= down.innerText) {
                cell.style.color = "red";
            }
        }
    }

}*/

function checkRepeatDigits(cell) {

    var isRepeated = false;
    if (cell && cell.innerText != "" && cell.style.color != "red") {
        var row = row = parseInt(cell.attributes.row.nodeValue);    
        var col = col = parseInt(cell.attributes.col.nodeValue);
        
        for (var c = -2; c < rowSize-2; c+=2) {        
            var o = next(row,c);
            if (c+2 != col && cell.innerText == o.innerText) {
                cell.style.color = o.style.color = "red";
                isRepeated |= true;
            }        
        }

        for (var r = -2;  r < rowSize-2; r+=2) {        
            var o = down(r, col);
            if (r+2 != row && cell.innerText == o.innerText) {
                cell.style.color = o.style.color = "red";
                isRepeated |= true;
            }
        }    

    }
    
    return isRepeated;
}

function checkAll() {
    
    if (!ChkCorrectness.checked) { 
        recoverColorAll();
        return; 
    }
    
    var cell;
//    var noNumber = true;
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
        var o = down(r, col);
        if (r+2 != row && o.style.color == "red" && o.innerText == repeat) {
            if (++rowRepeated > 1) { break; }
            repeatCell = o;
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
//    if (ChkCorrectness.checked) {}
    
    key = event.keyCode;
    var isRefreshKey = key == 116;    
    var isArrow = key >= 37 && key <= 40;
    var isNum = key >= 48 && key <= 57;    
    if (!currentCheckedCell && !isRefreshKey) { 
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
        
    if (key == 48 && currentCheckedCell.isSetByClient) {
        currentCheckedCell.innerText = "";
    } else if (isNum && currentCheckedCell.innerText == "" ){
        currentCheckedCell.innerText = v;    
        currentCheckedCell.isSetByClient = true;
        if (ChkCorrectness.checked) { 
            checkRepeatDigits(currentCheckedCell); 
            recoverColorOfSign(r,c);
        }        
    } else if (isNum && currentCheckedCell.innertText != "" && currentCheckedCell.isSetByClient) {
        var bac = currentCheckedCell.innerText;
        if (bac == v) { return false; }
        currentCheckedCell.innerText = v;
        currentCheckedCell.style.color = "#000000";
        if (ChkCorrectness.checked ) {
            checkRepeatDigits(currentCheckedCell);
            recoverColor(true, bac, r, c);
            checkInequalities(currentCheckedCell);
            recoverColorOfSign(r, c);
        }
    } else {
    }
    
    
  //  alert(key + "   " + (rowSize+1)/2 + "   " + event.value );
//    var cId = currentCheckedCell.id;
//    var rc = cId.split("_");        

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


function $(e) {
    return document.getElementById(e);
}