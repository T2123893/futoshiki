/**
 * $Id$
 * 
 * SOFT40051 - Object-oriented Programming in C++ Assignment
 *
 * This file is the result of my own work. Any contributions to the work by 
 * third parties, other than tutors, are stated clearly below this declaration. 
 * Should this statement prove to be untrue I recognise the right and duty of 
 * the Board of Examiners to take appropriate action in line with the university's 
 * regulations on assessment. 
 *
 * File:    Menu.h
 * Date:    12/01/2011
 * Name:    Huang Huang (Rock)
 * ID:      T2123893
 * Version: V1.0
 */

#pragma once

//#include <iostream>
#include "Display.h"
#include <string>

/**
 * This class provide a menu list.
 */
class Menu
{
private:
    enum { QUIT=113, ADD=97, CIRCLE=99, CONTENTS=99, DEL, PERFORMALL=102, PERFORM=112, RECTANGLE=114, SAVE, TRIANGLE, LOAD=108, MOVE, 
         EDIT=101, ZAP=122};
    Display _display;
    void setMenu(GWindow&, int subMenuHeight = 0, int menuIndex = -1);    

public:        
    void showContents(GWindow&);
    void showAdd(GWindow&);
    void show();
    Menu(void);
    ~Menu(void);
};
