# 2022.ArtGallary
Authors: Nathan Miller, Tyler Miller, Pavel Peev, Gary Yuen
Date of last edit: 6/8/2022

Hello and welcome to the online art gallery in which students and teachers can draw and write on canvas to share with each other.
Some of the functionalities of this software is to import and export PNG (currently does not support any other format currently).
This README will contain some information about how some of these features listed above work and any other specifics like
why some commented code is not deleted and certain limitations. All necessary c# code that you will need to edit can be found
under the directory ArtGallary/Assets/GalleryFiles. All prefabs can be found under ArtGallary/Assets/Resources/MyPrefabs.

PaintOnCanvas.cs:
This file houses all drawing, erasing, writing functionalities. There are several spots in code is commented out with another
comment describing why that code bellow it is commented out. Most of the time this comment shows alternate ways that 
may/may not be useful depending on the situation. The update function has several jobs, the first being the paint brush mask.
This shows the silhouette of where your cursor is on the canvas. The second is if you actually click on the canvas what will
be drawn or erased to the canvas. Another important thing to mention is that the size of the canvas is changeable but the
transformational scale must be changed to reflect the proper resolution of the canvas. Some functions like ChangeToWhite()
was for old implementations and can be ignored if the project does not get revert to an earlier stage.

IMPORTANT NOTE: Do not delete or modify anything named alphabet.png or alphabet2.png (or anything else named this for future cases).
Modifying this file i.e. shrinking or expanding will distort the letters due to improper dimensions. In case of deletion,
I have made sure that there are multiple copies of these files within this project. Make sure to turn off all compression and
to the non-power of two. The original creator of this file did not have a save/load from file explorer and had to manually
check this when a user entered a string. Some of this code may be commented out and not check to see if you are loading in
anything that contains alphabet.png.

RequestHandler.cs:
This is for when the student need help from the teacher. This references several UI elements that only the teacher can see. When a
student signals they need help several things will happen. In the eyes of the instructor the students canvas will grow to normal size,
the student canvas will now be displayed to the teacher, and the teacher UI will be updated according to the student requesting
help.

CanvasSpawner.cs:
This is for setting up the gallery space. More people mean the size of the gallery will be larger in order to accommodate more people.
The canvas will be set up in row, column formation.

Pavel_Player.cs:
This is where all player movement can be found for the gallery walk. The player can move along the x and z axis.

