import React from "react";

let bgs = [];
for (let i = 1; i <= 17; i++) {
  bgs.push(i + ".jpg");
}

const RandomBgGenerator = () => {
  let bg = bgs[Math.floor(Math.random() * bgs.length)];

  let baseUrl = document.getElementById("baseUrl").value;
  let port = document.getElementById("port").value;

  let url = `http://${baseUrl}:${port}/Content/bg/${bg}`;

  return url;
};

export default RandomBgGenerator;
