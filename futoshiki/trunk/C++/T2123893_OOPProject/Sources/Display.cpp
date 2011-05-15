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
 * File:    Display.cpp
 * Date:    12/01/2011
 * Name:    Huang Huang (Rock)
 * ID:      T2123893
 * Version: V1.0
 */


#include "stdafx.h"
#include "Display.h"


void Display::perform(GWindow &gw)
{    
    erase(gw);
    
    for (_it = _objList.begin(); _it != _objList.end(); _it++)
    {    
        (*_it)->perform(gw);   
    }    
}

void Display::quit(GWindow &gw)
{
    erase(gw);
    gw.writeText(200, 30, "Would you like to save the objects before quit? (Y/N)");
    string confirm = gw.readString(3);
    
    if (confirm == "Y")
    {
        save(gw);
    }
    else 
    {
        return;
    }
}

void Display::zap(GWindow &gw)
{
    erase(gw);
    gw.writeText(200, 15, "This is going to delete all the objects, do you want to save display to a file? (Y/N)");
    string confirm = gw.readString(3);
    if (confirm == "Y" ) 
    {
        save(gw);
    }
    for (_it = _objList.begin(); _it != _objList.end(); _it++)
    {    
        delete (*_it);
        _it = _objList.erase(_it);       
    }      
    gw.writeText(200, 55, "Objects have been deleted.");
    
}

void Display::load(GWindow &gw)
{
    erase(gw);
    gw.writeText(200, 30, "Load objects from file.");
    gw.writeText(200, 55, "Please enter a filename: ");
    string prefix = "./";
    string fileName = gw.readString(100);
    prefix += fileName;
    char *file = const_cast<char*>(prefix.c_str());
    fstream in(file, ios_base::in);
    if (!in) 
    {
        gw.writeText(200, 80, "Cannot find specified file.");
        return;
    }

    string tempStr;
    vector<string> attr;
	for (int i = 0; getline(in,tempStr); i++)
	{
		if (tempStr.length() > 0)
		{
            split(tempStr, ",", attr);
            if (attr[0] == "Triangle")
            {
                _objList.push_back(new Triangle());                
                _objList.back()->load(gw, attr);                    
            } 
            else if (attr[0] == "Rectangle")
            {
                _objList.push_back(new Rectangl());                
                _objList.back()->load(gw, attr);
            }
            else if (attr[0] == "Circle")
            {
                _objList.push_back(new Circle());                
                _objList.back()->load(gw, attr);            
            }
		}
		else 
		{
            gw.writeText(200, 80, "The file is empty.");			
		}
        attr.clear();
	}	
	in.close();
    gw.writeText(200, 105, "Successful loading file, Press C to look at all information.");

}

void Display::save(GWindow &gw)
{
    erase(gw);
    gw.writeText(200, 30, "Save objects to file.");
    gw.writeText(200, 55, "Please enter a filename: ");
    string prefix = "./";
    string fileName = gw.readString(100);
    prefix += fileName;
    char *file = const_cast<char*>(prefix.c_str());
    fstream out(file, ios_base::out);
    for (_it = _objList.begin(); _it != _objList.end(); _it++)
    {
        (*_it)->save(out);
    }
    out.close();
    gw.writeText(200, 80, "The objects have been saved.");
    
    
  
}

void Display::del(GWindow &gw)
{
    erase(gw);
    show(gw, 200);
    gw.writeText(200, 30, "Choose a ID to delete object: ");
    int index = gw.readInt();
    if (index < 1 || index > _objList.size())
    {
        gw.writeText(500, 30, "There is no this ID");   
    }
    int i = 1; 
    for (_it = _objList.begin(); _it != _objList.end(); _it++)
    {
        if (i == index) 
        {            
            delete (*_it);
            _objList.erase(_it);
            gw.writeText(200, 55, "The object has been deleted.");
            break;
        }
        i++;
    }    
}

void Display::edit(GWindow &gw)
{
    erase(gw);        
    show(gw, 280);       
    gw.writeText(200, 30, "Choose a ID to edit object: ");   
    int index = gw.readInt();
    if (index < 1 || index > _objList.size())
    {
        gw.writeText(500, 30, "There is no this ID");   
    }
    int i = 1; 
    for (_it = _objList.begin(); _it != _objList.end(); _it++)
    {
        if (i == index) 
        {
            (*_it)->prompt(gw);
        }
        i++;
    }
}

void Display::show(GWindow &gw, int top)
{  
    int index = 1;
    gw.writeText(200, top, "ID  OBJECT TYPE         ATTRIBUTES");
    gw.moveTo(200, top+13);
    gw.lineTo(820, top+13);
    for (_it = _objList.begin(); _it != _objList.end(); _it++)
    {
        gw.writeInt(200, top+=15, index);        
        (*_it)->show(gw);        
        index++;
    }
}

void Display::add(GWindow &gw, int choice)
{
    switch (choice)
    {
    case TRIANGLE:
        erase(gw);
        _objList.push_back(new Triangle());                
        _objList.back()->prompt(gw);
        break;
    case RECTANGLE:
        erase(gw);
        _objList.push_back(new Rectangl());
        _objList.back()->prompt(gw);
        break;
    case CIRCLE:
        erase(gw);
        _objList.push_back(new Circle());
        _objList.back()->prompt(gw);
        break;
    default:
        gw.writeText(220,3,"Please choose a menu item.");
    }  

}

void Display::erase(GWindow &gw) 
{
    gw.setPenColour(GColour(228,228,228));
    gw.rectangle(199,10,830,400);
    gw.setPenColour(BLACK);
}

void Display::split(string s, string seperator, vector<string> &rs)
{
    int pos;
    while( (pos = s.find_first_of(seperator)) != s.npos )
    {
        if(pos > 0)
        {
            rs.push_back(s.substr(0, pos));
        }
        s = s.substr(pos + 1);
    }

    if(s.length() > 0)
    {
        rs.push_back(s);
    }
}


void Display::checkEnteredID(GWindow &gw)
{
}

Display::Display(void)
{

}

Display::~Display(void)
{
}
