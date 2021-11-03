screen gameGallery():
    if Lightbox_image != "":
        $ lb_image = im.Scale("gallery/ " + Lightbox_image + ".jpg", 1280, 720)
        imagebutton:
            idle lb_image
            hover lb_image
            xalign 0.5
            yalign 0.5
            focus_mask True
            action SetVariable("Lightbox_image", "")
    else:
        frame:
            xpos 250
            ypos 100
            background None
            side ("r"):
                area (250,100,1820,980)
                viewport id "gallery":
                    draggable True mousewheel True
                    vpgrid:
                        cols 3
                        spacing 20

                        for q in galleryList:
                            $ qimage = "gallery/" + q + ".jpg"
                            $ lb_image = im.Scale(q, 320,180)
                            imagebutton:
                                idle lb_image
                                hover lb_image
                                action SetVariable("Lightbox_image", q)
