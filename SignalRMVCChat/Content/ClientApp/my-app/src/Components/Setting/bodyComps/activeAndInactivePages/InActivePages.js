import React, { Component } from "react";
import { DataHolder } from "./../../../../Help/DataHolder";
import { InputText } from "primereact/inputtext";
import { Button } from "react-bootstrap";
import { DataTable } from "primereact/datatable";
import { useEffect } from "react";
import { useState } from "react";
import { Dropdown } from "primereact/dropdown";
import { Column } from "primereact/column";
import { Col, Row } from "react-bootstrap";
import { _showError } from "../../../../Pages/LayoutPage";

const options = [
  { label: "شامل", value: "include" },
  { label: "برابر", value: "equals" },
];

const InActivePages = (props) => {
  const [text, setText] = useState();
  const [title, setTitle] = useState();
  const [applyType, setApplyType] = useState();
  const [list, setList] = useState();
  const [ran, setRan] = useState();

  useEffect(() => {
    setList(DataHolder.Setting.InActivePages);
  }, [DataHolder.Setting.InActivePages]);

  return (
    <>
      <Row>
        <Col>
          <label>آدرس صفحه</label>
          <InputText
            value={text}
            onChange={(e) => {
              setText(e.target.value);
            }}
          />
        </Col>
   
        <Col>
          <label>آدرس صفحه</label>
          <br />
          <Dropdown
            value={applyType}
            options={options}
            onChange={(e) => {
              setApplyType(e.value);
            }}
            placeholder="نوع اعمال"
          />
        </Col>
      </Row>

      <br />

      <Button
        onClick={() => {
          if (!text  || !applyType) {
            _showError("مقادیر صحیح نیست");
            return;
          }

          if (!DataHolder.Setting.InActivePages) {
            DataHolder.Setting.InActivePages = [];
          }

          DataHolder.Setting.InActivePages.push({
            Text: text,
            Title: title,
            ApplyType: applyType,
            rn: Math.random(),
          });

          setText("");
          setTitle("");
          setApplyType("");
          setList(DataHolder.Setting.InActivePages);

          setRan(Math.random());
        }}
      >
        افزودن
      </Button>

      <hr />
      <DataTable
        value={list}
        paginator
        paginatorTemplate="CurrentPageReport FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink RowsPerPageDropdown"
        currentPageReportTemplate="نمایش {first} از {last} کل {totalRecords}"
        rows={10}
        rowsPerPageOptions={[10, 20, 50]}
      >
        <Column field="Text" header="آدرس صفحه"></Column>
        <Column field="ApplyType" header="نوع اعمال"></Column>
      </DataTable>
    </>
  );
};

export default InActivePages;
