/*/ d3tools for sociew using d3.js library /*/
function DrawCircleImage(userid, images) {


    var margin = { top: 10, right: 5, bottom: 10, left: 5 };
    //console.log("type [" + typeof userid + "]");
    //console.log("selecting [#happiest-div-" + parseInt(userid) + "]");
    $("#happiest-div-" + userid + " img:last-child").remove()
    var svg = d3.select("#happiest-div-" + userid)
        .append("svg")
        .attr("width", "100%")
        .attr("height", 500)
        .append("g")
        .attr("transform", "translate(" + margin.left + "," + margin.top + ")");

    var defs = svg.append('svg:defs');
    var config = { "avatar_size": "250" };

    var currentX = 150;
    var currentY = 70;

    
    var index = 0;
    for (var v in images) {
        var currentImage = images[v];

        //console.log("--------");
        //console.log(currentImage);
        
        var score = currentImage.emotions.happiness * 100;
        var imageid = currentImage.photo.PhotoId;
        var patternId = "ptr_img_" + imageid;
        //console.log(images[v]);

        
        //console.log(imageid + " ==== " + patternId);
        //console.log("--------");

        var scale = currentImage.emotions.left / config.avatar_size;
        var scaledCenter = currentImage.emotions.left / 2 * scale;
        var scaledLeft = currentImage.emotions.left * scale;
        var scaledWidth = currentImage.emotions.width * scale;
        var possibleX = scaledLeft - ((scaledLeft - scaledWidth));
        if (possibleX < 0)
        {
            possibleX = 0;
        }


        // set pattern with image
        defs.append("svg:pattern")
            .attr("id", patternId)
            .attr("width", config.avatar_size)
            .attr("height", config.avatar_size)
            .attr("patternUnits", "userSpaceOnUse")
            .append("svg:image")
            .attr("xlink:href", '/Content/images/' + userid + "/image_" + imageid + "_small.jpg")
            .attr("width", config.avatar_size)
            .attr("height", config.avatar_size)
            .attr("x", 50)
            .attr("y", 10)
            .attr("viewBox", "0 0 450 450")
            .attr("preserveAspectRatio", "xMinYMin meet");

        // set circle with pattern
        // actually cerates circle with image
        svg.append("circle")
        .style("stroke", "gray")
        .style("fill", "url(#" + patternId + ")")
        .attr("r", 90)
        .attr("cx", currentX)
        .attr("cy", 130)
        .attr("width", 300)
        .attr("height", 300);

        // add score text rectangle (with percentages)
        // add rectangle
        var group = svg.append("g");
        var rect = group.append("rect")
            .attr("x", currentX - 20)
            .attr("y", 200)
            .attr("rx", 10)
            .attr("ry", 12)
            .style("fill", "gray")
            .style("fill-opacity", 0.5)
            .attr("width", 60)
            .attr("height", 35);

        // put text on rectangle
        group.append("text")
        .text(score.toFixed(2) + "%")
        //.style("stroke", "black")
        .style("font-size", "small")
        .attr("x", currentX - 15)
        .attr("y", 220);

        currentX += 250;
        ++index;
    }
    

    
}

function alo() {
    alert();
}