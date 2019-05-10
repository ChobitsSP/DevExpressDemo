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
