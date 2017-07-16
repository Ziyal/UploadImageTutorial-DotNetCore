# .NET Core Image Upload Tutorial || CURRENTLY WIP

This tutorial will show you how to upload multiple files to your sever with ASP.NET Core.

![Upload Demo](https://github.com/Ziyal/Gif-Land/blob/master/screenshots/demo.gif "Upload Demo")


### Let's Get Started!

#### Create Hosting Enviroment

In your controller inside your class we're going to create our hosting enviroment:

```cs
private IHostingEnvironment hostingEnv;

public HomeController(IHostingEnvironment env)
    {
        this.hostingEnv = env;
    }
```


#### Add Upload Form

In your .cshtml file we're going to create a form that will allow us to send our images to the controller: 

```cs
<form asp-action="UploadPhoto"
        asp-controller="Home"
        method="post"
        enctype="multipart/form-data">

    <input type="file" name="Images" multiple>

    <input type="submit" value="Upload Selected Files" class="btn">
</form>
```

**Notes:**

+ *asp-action='UploadPhoto'* is where we put the name ofthe method we want to send our images to.

+ *asp-controller='Home'* is the name of the controller that the function is in.

+ *input type="file"* tells us what we're allowing in the form and adding multiple to the end allows us to choose more than one image to upload.

#### Create UploadPhoto Method

Back in our contoller we're going to create a method that will save our images to our server and give us it's location to store in a database to reference later.

```cs
[HttpPost]
[Route("UploadPhoto")]
public IActionResult UploadPhoto(IList<IFormFile> Images) {
    
    long size = 0;
    var location = "";

    foreach(var file in Images) {
        var filename = ContentDispositionHeaderValue
                        .Parse(file.ContentDisposition)
                        .FileName
                        .Trim('"');
        location = $@"/images/uploaded/{filename}";
        filename = hostingEnv.WebRootPath + $@"\images\uploaded\{filename}";
        using (FileStream fs = System.IO.File.Create(filename)){
        file.CopyTo(fs);
        fs.Flush();
        }
    }

    return RedirectToAction("Index");
}  
```

**Notes:**

+ inside the loop our location variable is pointing to the folder we want our images to be stored in. Uploaded is the name of the folder I created to put them all in. This is also the variable you would want to store in your database to referance and display the image.


#### Using Statments Used
```cs 
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
```