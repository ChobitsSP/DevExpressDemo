'use strict';

function InitFileUrl(input) {
  const div = document.createElement('div');

  const inputDom = document.getElementById(input.name);
  inputDom.style = 'display: none;';
  inputDom.parentElement.appendChild(div);

  const app = new Vue({
    template: `
<el-upload
  class="upload-demo"
  action="/api/file/upload"
  :on-remove="handleRemove"
  :on-success="handleSuccess"
  :limit="1"
  :file-list="fileList">
  <el-button size="small" type="primary">点击上传</el-button>
  <div slot="tip" class="el-upload__tip">只能上传jpg/png文件，且不超过500kb</div>
</el-upload>
`,
    data() {
      return {
        fileList: []
      }
    },
    created() {
      const url = input.GetValue();

      if (url) {
        this.fileList = [{ url, name: url }];
      }
    },
    methods: {
      change(val) {
        input.SetValue(val);
      },
      handleRemove(file, fileList) {
        input.SetValue(null);
      },
      handleSuccess(response, file, fileList) {
        input.SetValue(response);
      }
    }
  });

  app.$mount(div);
}

function InitFilter() {
  /** @type{HTMLInputElement} */
  const input = document.getElementById("HiddenField1");

  const template = /* html */ `
<div>
<el-form :inline="true" size="small">
  <el-form-item label="multi select">
    <el-select v-model="filter.list1" multiple collapse-tags clearable>
      <el-option label="value1" :value="1" />
      <el-option label="value2" :value="2" />
      <el-option label="value3" :value="3" />
      <el-option label="value4" :value="4" />
    </el-select>
  </el-form-item>
</el-form>
</div>
`;

  new Vue({
    el: "#TableFilter",
    template: template,
    data: function () {
      return {
        filter: JSON.parse(input.value)
      };
    },
    watch: {
      filter: {
        handler: function (val, oldVal) {
          input.value = JSON.stringify(val);
        },
        deep: true
      }
    }
  });
}


window.onload = function () {
  InitFilter();
}
