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
 * File:    Menu.cpp
 * Date:    12/01/2011
 * Name:    Huang Huang (Rock)
 * ID:      T2123893
 * Version: V1.0
 */


#include "stdafx.h"


void Menu::showContents(GWindow &gw)
{
    setMenu(gw);
    _display.show(gw);
}

void Menu::showAdd(GWindow &gw)
{    
    setMenu(gw, 100, 0);
    GFont contentFontStyle("Calibri", 12);
    gw.setFont(&contentFontStyle);
    gw.writeString(37, 55, "[T] Triangle");
    gw.writeString(37, 70, "[R] Rectangle");
    gw.writeString(37, 85, "[C] Circle");
      
    GFont promptFontStyle("Calibri", 14);
    gw.setFont(&promptFontStyle);
    GKey choice = QUIT; 
	do 
    {        
        choice = Keyboard.waitKey();
        _display.add(gw, choice);

	} while (QUIT != choice);	

    setMenu(gw);
}


void Menu::show() 
{	    
	GWindow Gwin(850,500,false);
	
	// Clear the Gwin window     
	GColour mainBg(228, 228, 228);
    Gwin.clear(mainBg);
    Gwin.setTitle("SOFT40051 - OOP in C++ Assignment");
	Gwin.setPenColour(BLACK);	       

    setMenu(Gwin);

    GKey choice = QUIT;    
	do 
    {
        choice = Keyboard.waitKey();
        Gwin.writeInt(200,200,choice);
       
        switch (choice)
        {
        case ADD:
            showAdd(Gwin);
            break;
        case CONTENTS:
            showContents(Gwin);            
            break;
        case EDIT:
            _display.edit(Gwin);
            break;
        case DEL:
            _display.del(Gwin);
            break;
        case PERFORM:
            _display.perform(Gwin);
            break;
        case PERFORMALL:
            Gwin.writeText(200,400, "perform all");
            break;
        case SAVE:
            _display.save(Gwin);
            break;
        case LOAD:
            _display.load(Gwin);          
            break;
        case MOVE:
            Gwin.writeText(67,23,"move");
        case ZAP:
            _display.zap(Gwin);
        case QUIT:
            _display.quit(Gwin);
            break;
        default:
            Gwin.writeText(200,3,"Please choose a menu item.");
        }

	} while (QUIT != choice);	


	// Finally, wait for a key to be pressed
	//Keyboard.waitKey();
}

void Menu::setMenu(GWindow &gw, int subMenuHeight, int menuIndex)
{
    GFont titleFontStyle("Cambria",14, GwinFontStyles::BOLD);
    gw.setFont(&titleFontStyle);
        
    // x1=10 y1=35
    GColour bg(228, 228, 228);
    gw.clear(bg);    
    gw.setPenColour(BLACK);	
    gw.writeText(10, 10, "Main Menu");

    int menuSize = 10;
    string menuItem[] = {
        "(A) ADD", "(D) DELETE", "(C) CONTENTS", "(P) PERFORM", 
        "(F) PERFORM ALL", "(S) SAVE", "(L) LOAD", "(E) EDIT", 
        "(M) MOVE", "(Z) ZAP", "(Q) QUIT"
    };    

    int y = 35;
    for (int i = 0; i < menuSize; i++)
    {
        if ( menuIndex+1 == i && subMenuHeight != 0) 
        {
            y = subMenuHeight;
        }        
        else 
        {
            y = 0==i ? y : y+=22;
        }
        gw.writeString(10, y, menuItem[i]);
    
    }

}

Menu::Menu(void)
{
}

Menu::~Menu(void)
{
}





