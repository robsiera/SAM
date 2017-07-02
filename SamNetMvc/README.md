# SamNetMvc

Some of the original [SAM samples](https://github.com/jdubray/sam-samples/), migrated onto a .NET back-end.

## Demo 1 
implements https://github.com/jdubray/sam-samples/tree/master/crud-blog with client-side model.

This is a simple demo. All the logic has been implemented on the client.

## Demo 2
implements https://github.com/jdubray/sam-samples/tree/master/crud-blog with server-side model.
This is exactly the same demo as Demo1, but in Demo2.cshtml I commented out the lines to enable server side support (just compare files Demo1.cshtml and Demo2.cshtml to see what I mean).
Hence I created a Demo2 apicontroller (Demo2Controller.cs) and migrated all the relevant code from the blog.js into this controller. 

Not all features are working, as there is no Db attached. But it gives an idea how this might work.

In this demo Actions are still client side, but Presenter, Model and the State function are server side.
