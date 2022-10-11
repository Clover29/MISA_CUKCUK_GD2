<template>
  <TheAlert
    v-if="isShowAlert"
    :showAlert="showAlert"
    :msg="alertMsg"
    :alertType="alertType"
  />
  <div class="container">
    <div class="content__tittle">
      <div class="title">Nguyên vật liệu</div>
      <button class="btn">
        <i class="fa-solid fa-bullhorn icon__horn"></i>Phản hồi
      </button>
    </div>
    <div class="content__table">
      <LoadingVue v-show="isLoad" />
      <div class="table__option">
        <div class="label color__black" @click="showForm(true)">
          <i class="fa-solid fa-file-circle-plus icon__add"></i>Thêm
        </div>
        <div class="label color__black" @click="onDuplicateClick()">
          <i class="fa-regular fa-copy icon__duplicate"></i>Nhân Bản
        </div>
        <div class="label color__black" @click="onUpdateClick()">
          <i class="fa-solid fa-pen-to-square icon__add"></i>Sửa
        </div>
        <div class="label color__black" @click="onDeleteClick(true)">
          <i class="fa-solid fa-xmark icon__delete"></i>Xóa
        </div>
        <div class="label color__black" @click="onRefreshClick">
          <i class="fa-solid fa-arrows-rotate icon__add"></i>Nạp
        </div>
      </div>
      <table class="table__data">
        <tr>
          <th>
            Mã nguyên vật liệu
            <div class="table__text--search">
              <SelectionBox
                ref="MaterialCode"
                :isButtonShow="true"
                :isSelectShow="false"
                :option_class="'fillter__position'"
                :option="fillterOption"
                :optionType="fillterType"
                :optionID="fillterOption[0].id"
                @fillter="getFillterOption('MaterialCode')"
              />
              <input
                v-model="MaterialCode"
                type="text"
                class="input__search"
                @keydown.enter="getFillterOption('MaterialCode')"
              />
            </div>
          </th>
          <th>
            Tên nguyên vật liệu
            <div class="table__text--search">
              <SelectionBox
                ref="MaterialName"
                :isButtonShow="true"
                :isSelectShow="false"
                :option_class="'fillter__position'"
                :option="fillterOption"
                :optionType="fillterType"
                :optionID="fillterOption[0].id"
                @fillter="getFillterOption('MaterialName')"
              />
              <input
                type="text"
                class="input__search"
                v-model="MaterialName"
                @keydown.enter="getFillterOption('MaterialName')"
              />
            </div>
          </th>
          <th>
            Tính chất
            <div class="table__text--search">
              <SelectionBox
                ref="Feature"
                :isButtonShow="true"
                :isSelectShow="false"
                :option_class="'fillter__position'"
                :option="fillterOption"
                :optionType="fillterType"
                :optionID="fillterOption[0].id"
                @fillter="getFillterOption('Feature')"
              />
              <input
                type="text"
                class="input__search"
                v-model="Feature"
                @keydown.enter="getFillterOption('Feature')"
              />
            </div>
          </th>
          <th title="Đơn vị tính">
            ĐVT
            <div class="table__text--search">
              <SelectionBox
                ref="UnitName"
                :isButtonShow="true"
                :isSelectShow="false"
                :option_class="'fillter__position'"
                :option="fillterOption"
                :optionType="fillterType"
                :optionID="fillterOption[0].id"
                @fillter="getFillterOption('UnitName')"
              />
              <input
                type="text"
                class="input__search"
                v-model="UnitName"
                @keydown.enter="getFillterOption('UnitName')"
              />
            </div>
          </th>
          <th>
            Nhóm nguyên vật liệu
            <div class="table__text--search">
              <SelectionBox
                ref="CategoryName"
                :isButtonShow="true"
                :isSelectShow="false"
                :option_class="'fillter__position'"
                :option="fillterOption"
                :optionType="fillterType"
                :optionID="fillterOption[0].id"
                @fillter="getFillterOption('CategoryName')"
              />
              <input
                type="text"
                class="input__search"
                v-model="CategoryName"
                @keydown.enter="getFillterOption('CategoryName')"
              />
            </div>
          </th>
          <th>
            Ghi chú
            <div class="table__text--search">
              <SelectionBox
                ref="Description"
                :isButtonShow="true"
                :isSelectShow="false"
                :option_class="'fillter__position'"
                :option="fillterOption"
                :optionType="fillterType"
                :optionID="fillterOption[0].id"
                @fillter="getFillterOption('Description')"
              />
              <input
                type="text"
                class="input__search"
                v-model="Description"
                @keydown.enter="getFillterOption('Description')"
              />
            </div>
          </th>
          <th>
            Ngừng theo dõi
            <SelectionBox
              ref="Status"
              :isSelectShow="true"
              :div_class="'table__text--search'"
              :input_class="'select__input col_width_100'"
              :option="statusOption"
              :optionType="statusType"
              :value="statusOption[0].text"
              :optionID="statusOption[0].id"
              @Status="setStatus"
            />
          </th>
        </tr>
        <tr
          v-for="item in materialList"
          :key="item.materialID"
          @click="getMaterialSelected(item)"
          @dblclick="onUpdateClick()"
          :class="{ row__selected: MaterialID == item.materialID }"
        >
          <td>{{ item.materialCode }}</td>
          <td>{{ item.materialName }}</td>
          <td>{{ item.feature }}</td>
          <td>{{ item.unitName }}</td>
          <td>{{ item.categoryName }}</td>
          <td class="text__elipsis" :title="item.description">
            {{ item.description }}
          </td>
          <td class="checkbox__col">
            <input
              type="checkbox"
              :checked="item.status == 2"
              :value="item.status"
            />
          </td>
        </tr>
      </table>
    </div>
    <MaterialPaging
      ref="MaterialPaging"
      :totalRecord="totalPageRecord"
      @LoadData="loadData"
      :totalPage="totalPage"
      :materialFillter="materialFillter"
      v-model:PageNumber="currentPage"
      v-model:pageSize="pageSize"
    />
    <MaterialForm
    ref="MaterialForm"
      v-if="isShowForm"
      :Title="formTitle"
      :showForm="showForm"
      @LoadData="loadData"
      :MaterialID="MaterialID"
      :ActionType="actionType"
      @showAlert="showAlert"
    />
  </div>
  <PopUp
    :message="message"
    v-if="isShowPopUp"
    @onYesClick="deleteMaterial"
    :closePopUp="onDeleteClick"
    :PopUpType="popUpType"
  />
</template>
<style scoped>
@import url(container.css);
</style>
<script>
import axios from "axios";
import { MISAEnum } from "../../../js/Enum.js";
import { Resources } from "../../../js/Resources.js";
import MaterialForm from "../../base/Form/MaterialForm.vue";
import MaterialPaging from "../../base/Paging/MaterialPaging.vue";
import SelectionBox from "../../base/SelectionBox/SelectionBox.vue";
import PopUp from "../../base/PopUp/ThePopUp.vue";
import LoadingVue from "@/components/base/Loading/LoadData.vue";
import TheAlert from "../../base/Alert/TheAlert.vue";
export default {
  name: "TheContainer",
  components: {
    MaterialForm,
    MaterialPaging,
    SelectionBox,
    PopUp,
    LoadingVue,
    TheAlert,
  },
  data() {
    return {
      formTitle:"",//tittle form tương ứng
      isShowAlert: false, // kiểm tra trạng thái của alert có được hiển thị hay không
      alertMsg: "", //thông tin hiển thị trên alert
      alertType: "", // quyết định loại alert cần hiển thị
      isLoad: false, // ẩn hiện loading
      pageSize: 10, // số bản ghi hiển thị trong 1 trang
      message: "", // thông tin của pop up
      popUpType: 0, // loại pop up được hiển thị
      isShowPopUp: false, // ẩn hiện pop up
      isShowForm: false, //hiển thị form thông tin nguyên liệu,
      isUpdate: false, // check trạng thái khi nào update
      MaterialID: "", //id nguyên vật liệu
      MaterialCode: "", // mã nguyên vật liệu
      MaterialName: "", // tên nguyên vật liệu
      Feature: "", // tính chất
      UnitName: "", // tên đơn vị
      CategoryName: "", // tên nhóm nguyên vật liệu
      Description: "", // mô tả
      material: {}, // nguyên vật liệu
      materialFillter: {
        MaterialCode: "",
        MaterialName: "",
        Feature: "",
        UnitName: "",
        CategoryName: "",
        Description: "",
        Status: 1,
      }, //danh sách các thuộc tính lọc nguyên vật liệu
      materialList: [], //danh sách nguyên vật liệu,
      totalPageRecord: 0, // tổng số bản ghi được tìm thấy
      currentPage: 1, // trang hiện tại
      totalPage: 0, // tổng số trang
      statusOption: [
        {
          id: 1,
          value: MISAEnum.StatusOption.Using.value,
          text: MISAEnum.StatusOption.Using.text,
        },
        {
          id: 2,
          value: MISAEnum.StatusOption.NotUsing.value,
          text: MISAEnum.StatusOption.NotUsing.text,
        },
      ], // list trạng thái
      statusType: MISAEnum.OptionType.Status, //loại trạng thái
      fillterOption: [
        {
          id: 1,
          value: MISAEnum.FillterOption.Contain.value,
          text: MISAEnum.FillterOption.Contain.text,
        },
        {
          id: 2,
          value: MISAEnum.FillterOption.Equal.value,
          text: MISAEnum.FillterOption.Equal.text,
        },
        {
          id: 3,
          value: MISAEnum.FillterOption.StartWidth.value,
          text: MISAEnum.FillterOption.StartWidth.text,
        },
        {
          id: 4,
          value: MISAEnum.FillterOption.EndWidth.value,
          text: MISAEnum.FillterOption.EndWidth.text,
        },
        {
          id: 5,
          value: MISAEnum.FillterOption.NotContain.value,
          text: MISAEnum.FillterOption.NotContain.text,
        },
      ], // lựa chọn lọc
      fillterType: MISAEnum.OptionType.Fillter, //loại lọc muốn thực hiện
      fillterText: MISAEnum.FillterOption.Contain.value, //text muốn search
      actionType: 0, // loại chức năng muôn thực hiện
    };
  },
  methods: {
    /**
     * Thông báo hiển thị khi người dùng thực hiện xong 1 thao tác
     * @param isShow : giá trị dùng để thể hiện trạng thái ẩn hiện của alert
     * @param msg: message thông báo
     * @param type: kiểu thông báo - success: thành công, error: thất bại
     * AUTHOR: YENVTH
     * CreatedDate: 10/10/2022
     */
    showAlert(isShow, msg, type) {
      try {
        this.isShowAlert = isShow;
        if (msg && type) {
          this.alertMsg = msg;
          this.alertType = type;
        }
        if (this.isShowAlert) {
          setTimeout(() => {
            this.isShowAlert = false;
          }, 5000);
        }
      } catch (error) {
        console.log(error);
      }
    },
    /**
     *Hàm mở form
     * AUTHOR: YENVTH
     * CreatedDate:28/09/2022
     */
    showForm(isShow) {
      this.isShowForm = isShow;
      if(this.$refs && this.$refs.MaterialForm ) this.$refs.MaterialForm.pageSize = this.pageSize;
      this.formTitle = Resources.ADD_TITLE;
      this.actionType = MISAEnum.ActionType.Insert;
    },
    /**
     *Hàm load danh sách nguyên vật liệu
     * AUTHOR: YENVTH
     * CreatedDate:03/10/2022
     */
    loadData(pageSize, pageNumber, materialFillter) {
      this.isLoad = true;
      if(materialFillter){
          axios
        .get(
          Resources.DOMAIN +
            Resources.API_VER +
            Resources.MATERIAL_PATH +
            `/Fillter?MaterialCode=${materialFillter.MaterialCode}&MaterialName=${materialFillter.MaterialName}&Feature=${materialFillter.Feature}&UnitName=${materialFillter.UnitName}&CategoryName=${materialFillter.CategoryName}&Description=${materialFillter.Description}&Status=${materialFillter.Status}&pageSize=${pageSize}&pageNumber=${pageNumber}`
        )
        .then((response) => {
          if (response && response.data && response.data.data) {
            this.materialList = response.data.data;
            this.MaterialID = response.data.data[0].materialID;
            this.material = response.data.data[0];
            this.totalPage = response.data.totalPage;
            this.totalPageRecord = response.data.totalRecord;
            this.currentPage = response.data.currentPage;
            this.isLoad = false;
          }
        })
        .catch((error) => {
          console.log(error.response);
          this.isLoad = false;
          this.isShowPopUp = true;
          this.popUpType = MISAEnum.PopUpType.Error;
          this.message = [];
          this.message.push({
            msg: Resources.UNDEFINED_ERROR,
          });
        });
      }
    
    },
    /**
     *Hàm set trạng thái
     * AUTHOR: YENVTH
     * CreatedDate:03/10/2022
     */
    setStatus(data) {
      try {
        if(this.materialFillter){
           this.materialFillter.Status = data;
        this.$refs.MaterialPaging.pageNumber = 1;
        this.loadData(this.pageSize, 1, this.materialFillter);
        }
      } catch (error) {
        console.log(error);
      }
    },
    /**
     *Hàm lấy ra lựa chọn lọc
     * AUTHOR: YENVTH
     * CreatedDate:03/10/2022
     */
    getFillterOption(value) {
      try {
        if(this.$refs[value] && this.materialFillter ){
           if (this.$refs[value].fillterText != MISAEnum.FillterOption.StartWidth.value) {
          this.materialFillter[value] =
            this.$refs[value].fillterText + this[value];
        } else {
          this.materialFillter[value] = Resources.TEXT_ADD + this[value];
        }
        }
        this.currentPage = 1;
       if(this.$refs.MaterialPaging) this.$refs.MaterialPaging.pageNumber = 1;
        this.loadData(this.pageSize, 1, this.materialFillter);
      } catch (error) {
        console.log(error);
      }
    },
    /**
     *Hàm lấy thông tin nguyên vật liệu khi click
     * AUTHOR: YENVTH
     * CreatedDate:03/10/2022
     */
    getMaterialSelected(material) {
      try {
        if(material){
            this.MaterialID = material.materialID;
        this.material = material;
        }
      } catch (error) {
        console.log(error);
      }
    },
    /**
     *Hàm mở form khi click button sửa
     * AUTHOR: YENVTH
     * CreatedDate:03/10/2022
     */
    onUpdateClick() {
      try {
        if (this.MaterialID) {
          this.isShowForm = true;
          this.actionType = MISAEnum.ActionType.Update;
          this.formTitle = Resources.UPDATE_TITLE;
        }
      } catch (error) {
        console.log(error);
      }
    },
    /**
     *Hàm thực hiện chức năng nhân bản
     * AUTHOR: YENVTH
     * CreatedDate:03/10/2022
     */
    onDuplicateClick() {
      try {
        if (this.MaterialID) {
          this.isShowForm = true;
          this.actionType = MISAEnum.ActionType.Duplicate;
          this.formTitle = Resources.ADD_TITLE;
        }
      } catch (error) {
        console.log(error);
      }
    },
    /**
     *Hàm xóa nguyên vật liệu
     * AUTHOR: YENVTH
     * CreatedDate:03/10/2022
     */
    deleteMaterial() {
      axios
        .delete(
          Resources.DOMAIN +
            Resources.API_VER +
            Resources.MATERIAL_PATH +
            `/${this.MaterialID}`
        )
        .then((response) => {
          if (response.data == this.MaterialID) {   
            this.showAlert(
              true,
              Resources.DELETE_SUCCESS,
              MISAEnum.Status.Success
            );
            this.loadData(this.pageSize, 1, this.materialFillter);
          }
        })
        .catch((error) => {
          console.log(error.response);
          this.isLoad = false;
          this.isShowPopUp = true;
          this.popUpType = MISAEnum.PopUpType.Error;
          this.message = [];
          this.message.push({
            msg: Resources.UNDEFINED_ERROR,
          });
        });
    },
    /**
     *Hàm mở form confirm trước khi xóa
     * AUTHOR: YENVTH
     * CreatedDate:03/10/2022
     */
    onDeleteClick(isShow) {
      try {
        this.message = [];
        this.isShowPopUp = isShow;
        this.message.push({
          msg: `Bạn có chắc muốn xóa Nguyên vật liệu << ${this.material.materialCode} - ${this.material.materialName}>> không?`,
        });
        this.popUpType = MISAEnum.PopUpType.Alert;
      } catch (error) {
        console.log(error);
      }
    },
    onRefreshClick() {
      try {
        this.MaterialCode = "";
        this.MaterialName = "";
        this.Feature = "";
        this.UnitName = "";
        this.CategoryName = "";
        this.Description = "";
     if(this.$refs.Status) 
       this.$refs.Status.optionValue = this.statusOption[0].text;
        this.materialFillter = {
          MaterialCode: "",
          MaterialName: "",
          Feature: "",
          UnitName: "",
          CategoryName: "",
          Description: "",
          Status: 1,
        };
        this.loadData(this.pageSize, 1, this.materialFillter);
      if( this.$refs.MaterialPaging)  this.$refs.MaterialPaging.pageNumber = 1;
      } catch (error) {
        console.log(error);
      }
    },
  },
  created() {
    this.loadData(10, 1, this.materialFillter);
  },
};
</script>
